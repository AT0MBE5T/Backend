using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Infrastructure.Hubs;

public class MessageHub: Hub<IChatClient>
{
    private readonly IDistributedCache _cache;
    private readonly IChatService _chatService;
    private readonly ICommentService _commentService;
    private readonly IQuestionService _questionService;
    private readonly IAnswerService _answerService;
    private readonly IAnnouncementsService _announcementsService;
    private readonly WebPushService _webPushService;
    private readonly UserManager<User> _userManager;
    
    public MessageHub(
        IDistributedCache cache,
        ICommentService commentService,
        IAnswerService answerService,
        IQuestionService questionService,
        WebPushService webPushService,
        UserManager<User> userManager,
        IAnnouncementsService announcementsService,
        IChatService chatService)
    {
        _cache = cache;
        _commentService = commentService;
        _answerService = answerService;
        _questionService = questionService;
        _webPushService = webPushService;
        _userManager = userManager;
        _announcementsService = announcementsService;
        _chatService = chatService;
    }

    public async Task JoinChat(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
        var userId = Context.User.GetUserId();
        
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };

        await _cache.SetStringAsync($"chat_{userId}", JsonSerializer.Serialize(connection), options);
    }
    
    public async Task JoinCommonChat(string userName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "common_chat");
        var userId = Context.User.GetUserId();
        
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };

        await _cache.SetStringAsync($"common_chat_{userId}", JsonSerializer.Serialize(userName), options);
    }
    
    public async Task JoinRoom(UserConnection connection)
    {
        var userId = Context.User.GetUserId();
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
        var stringConnection = JsonSerializer.Serialize(connection);
        
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };
        
        await _cache.SetStringAsync(Context.ConnectionId, stringConnection, options);
        await _cache.SetStringAsync($"active_room_{userId}", connection.ChatRoom,
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) });
    }
    
    public async Task JoinRoomWpf(UserConnection connection)
    {
        var userId = Context.User.GetUserId();
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
        var stringConnection = JsonSerializer.Serialize(connection);
        
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };
        
        await _cache.SetStringAsync(Context.ConnectionId, stringConnection, options);
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.GetUserId();
        await _cache.RemoveAsync(Context.ConnectionId);
        await _cache.RemoveAsync($"active_room_{userId}");
        await _cache.RemoveAsync($"chat_{userId}");
        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task NotifyUpdateFullOfferAsync(AnnouncementFullDto offerDto)
    {
        var userId = Context.User.GetUserId();

        var connectionJson = await _cache.GetStringAsync($"active_room{userId}");
        if (connectionJson is null) return;

        var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        if (connection is null) return;

        // await Clients.Group(offerDto.Id.ToString())
        //     .UpdateFullOffer(offerDto);
    }
    
    public async Task SendMessage(Guid chatId, string message, string userName, Guid? offerId)
    {
        var userId = Context.User.GetUserId();

        var connectionJson = await _cache.GetStringAsync($"chat_{userId}");
        if (connectionJson is null) return;

        var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        if (connection is null) return;

        var messageId = await _chatService.AddMessage(userId, chatId, message);
        if (messageId == Guid.Empty) return;
        
        await Clients.Group(chatId.ToString())
            .ReceiveMessage(userId,
                connection.UserName,
                message,
                chatId);

        var obj = await _chatService.GetMessageById(messageId);

        if (obj is null)
            return;
        
        await Clients.Group("messages_global").ReceiveMessageWPF(obj);
        
        var participants = await _chatService.GetChatParticipants(chatId);
        
        var receiverIds = participants.Where(p => p != userId);
        
        foreach (var receiverId in receiverIds)
        {
            var activeRoom = await _cache.GetStringAsync($"chat_{receiverId}");

            if (activeRoom is not null)
                continue;

            await _webPushService.SendNotificationToUserAsync(receiverId, $"[{userName}] {message}", $"/chats/{chatId}", "New message");
        }
        
        foreach (var participantId in participants)
        {
            await Clients.Group(participantId.ToString())
                .UpdateChatList(
                        chatId,
                        offerId,
                        connection.UserName,
                        message
                    );
        }
    }
    
    public async Task SendMessageInCommon(string message, string userName)
    {
        var userId = Context.User.GetUserId();

        var chatId = new Guid("74679c97-aa14-444e-b3ae-9a6d8d01399f");

        var messageId = await _chatService.AddMessage(userId, chatId, message);
        if (messageId == Guid.Empty) return;
        
        await Clients.Group(chatId.ToString())
            .ReceiveMessage(userId,
                userName,
                message,
                chatId);
        
        await Clients.Group("common_chat")
            .UpdateChatList(
                chatId,
                null,
                userName,
                message
            );
        
        var obj = await _chatService.GetMessageById(messageId);

        if (obj is null)
            return;
        
        await Clients.Group("messages_global").ReceiveMessageWPF(obj);
    }
    
    public async Task LeaveComment(Guid chatId, string message, string userName)
    {
        var userId = Context.User.GetUserId();
        
        var connectionJson = await _cache.GetStringAsync(Context.ConnectionId);
        if (connectionJson is null) return;
        var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        
        if (connection is null) return;

        var objForAdding = new CommentDto
        {
            UserId = userId,
            AnnouncementId = chatId,
            Text = message,
            CreatedAt = DateTime.UtcNow
        };
        
        var success = await _commentService.InsertCommentAsync(objForAdding);

        var offer = await _announcementsService.GetAnnouncementFullById(new AnnouncementInfoCommandDto(chatId, userId));

        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (offer is null || user is null)
            return;
        
        if (success is not null)
        {
            await Clients.Group(chatId.ToString()).ReceiveComment(success.Value, chatId, userName, message);
            var commentWpf = new CommentGridDto()
            {
                Id = success.Value,
                Text = message,
                Author = user.UserName ?? string.Empty,
                CreatedAt = DateTime.UtcNow,
                AnnouncementId = chatId,
                StatementTitle = offer.Title
            };
            await Clients.Group("comments_global").ReceiveCommentWPF(commentWpf);
        }
    }
    
    public async Task SendQuestion(Guid chatId, string message, string userName)
    {
        var userId = Context.User.GetUserId();
        
        var connectionJson = await _cache.GetStringAsync(Context.ConnectionId);
        if (connectionJson is null) return;
        var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        
        if (connection is null) return;

        var questionDto = new QuestionDto
        {
            UserId = userId,
            AnnouncementId = chatId,
            Text = message,
            CreatedAt = DateTime.UtcNow,
            Id = Guid.NewGuid()
        };
        
        var questionId = await _questionService.InsertQuestionAsync(questionDto);

        if (questionId is null)
            return;
        
        var offer = await _announcementsService.GetAnnouncementFullById(new AnnouncementInfoCommandDto(chatId, userId));

        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (offer is null || user is null)
            return;

        var questionAnswerDto = new QuestionAnswerGridDto
        {
            AnnouncementId = chatId,
            QuestionId = questionId.Value,
            AnnouncementName = offer.Title,
            CreatedAtQuestion = DateTime.UtcNow,
            CreatedByQuestion = user.UserName ?? string.Empty,
            TextQuestion = message,
            AnswerId = null,
            CreatedAtAnswer = null,
            CreatedByAnswer = string.Empty,
            TextAnswer = string.Empty
        };
        
        await Clients.Group(chatId.ToString()).ReceiveQuestion(chatId, questionId.Value, userName, message);
        await Clients.Group("questions_global").ReceiveQuestionWPF(questionAnswerDto);
        
        var authorId = await _announcementsService.GetAuthorOfferIdByQuestionId(questionId.Value);
        if (authorId == Guid.Empty)
            return;
        
        var activeRoom = await _cache.GetStringAsync($"active_room_{authorId}");

        if (activeRoom is not null)
            return;
        
        await _webPushService.SendNotificationToUserAsync(authorId, $"[{userName}] {message}", $"/offers/{chatId}/questions","New question");
    }
    
    public async Task SendAnswer(Guid chatId, Guid questionId, string message, string userName)
    {   
        var userId = Context.User.GetUserId();
        
        var connectionJson = await _cache.GetStringAsync(Context.ConnectionId);
        if (connectionJson is null) return;
        var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        
        if (connection is null) return;

        var answerDto = new AnswerDto
        {
            UserId = userId,
            QuestionId = questionId,
            Text = message,
            CreatedAt = DateTime.UtcNow,
            Id = Guid.NewGuid()
        };
    
        var answerId = await _answerService.InsertAnswerAsync(answerDto);
        if (answerId is null)
            return;
        
        var offer = await _announcementsService.GetAnnouncementFullById(new AnnouncementInfoCommandDto(chatId, userId));

        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (offer is null || user is null)
            return;

        var questionAnswerDto = new QuestionAnswerGridDto
        {
            AnnouncementId = chatId,
            QuestionId = questionId,
            AnnouncementName = offer.Title,
            CreatedAtQuestion = DateTime.UtcNow,
            CreatedByQuestion = user.UserName ?? string.Empty,
            TextQuestion = message,
            AnswerId = answerId.Value,
            CreatedAtAnswer = DateTime.UtcNow,
            CreatedByAnswer = userName,
            TextAnswer = message
        };
        
        await Clients.Group(chatId.ToString()).ReceiveAnswer(answerId.Value, questionId, userName, message);
        await Clients.Group("questions_global").ReceiveAnswerWPF(questionAnswerDto);

        var questionUserId = await _questionService.GetQuestionUserIdByAnswerId(answerId.Value);
        if (questionUserId == Guid.Empty)
            return;
        
        var activeRoom = await _cache.GetStringAsync($"active_room_{questionUserId}");

        if (activeRoom is not null)
            return;
        
        await _webPushService.SendNotificationToUserAsync(questionUserId, $"[{userName}] {message}", $"/offers/{chatId}/questions", "New answer");
    }
    
    public async Task DeleteComment(Guid chatId, Guid commentId)
    {
        var userId = Context.User.GetUserId();
        
        // var connectionJson = await _cache.GetStringAsync(Context.ConnectionId);
        // if (connectionJson is null) return;
        // var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        //
        // if (connection is null) return;


        var res = await _commentService.DeleteByCommentIdAsync(commentId, userId);

        if (res)
        {
            await Clients.Group(chatId.ToString()).DeleteComment(commentId);
            await Clients.Group("comments_global").DeleteCommentWPF(commentId);
        }
    }
    
    public async Task DeleteAnswer(Guid chatId, Guid answerId)
    {
        var userId = Context.User.GetUserId();
        
        // var connectionJson = await _cache.GetStringAsync(Context.ConnectionId);
        // if (connectionJson is null) return;
        // var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        //
        // if (connection is null) return;


        var res = await _answerService.DeleteByAnswerIdAsync(answerId, userId);

        if (res)
        {
            await Clients.Group(chatId.ToString()).DeleteAnswer(answerId);
            await Clients.Group("questions_global").DeleteAnswerWPF(answerId);
        }
    }
    
    public async Task DeleteQuestion(Guid chatId, Guid questionId)
    {
        var userId = Context.User.GetUserId();
        
        // var connectionJson = await _cache.GetStringAsync(Context.ConnectionId);
        // if (connectionJson is null) return;
        // var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        //
        // if (connection is null) return;


        var res = await _questionService.DeleteByQuestionIdAsync(questionId, userId);

        if (res)
        {
            await Clients.Group(chatId.ToString()).DeleteQuestion(questionId);
            await Clients.Group("questions_global").DeleteQuestionWPF(questionId);
        }
    }
    
    public async Task LeaveGroup(string groupName)
    {
        await _cache.RemoveAsync(Context.ConnectionId);
        await _cache.RemoveAsync("active_room_" + Guid.Empty);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}