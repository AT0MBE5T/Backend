using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Infrastructure.Hubs;

public interface IChatClient
{
    public Task ReceiveMessage(Guid userId, string userName, string message, Guid chatId);
    public Task ReceiveComment(Guid commentId, Guid offerId, string userName, string message);
    public Task ReceiveQuestion(Guid announcementId, Guid questionId, string userName, string message);
    public Task ReceiveAnswer(Guid answerId, Guid questionId, string userName, string message);
    public Task UpdateChatList(Guid chatId, Guid offerId, string userName, string message);
    public Task ReceiveOffer(AnnouncementShortDto offer);
    public Task UpdateOffer(AnnouncementShortDto offer);
    public Task DeleteOffer(Guid offerId);
    public Task DeleteAnswer(Guid answerId);
    public Task DeleteQuestion(Guid questionId);
    public Task DeleteComment(Guid commentId);
}

public record UserConnection(string ChatRoom, string UserName);

public class MessageHub: Hub<IChatClient>
{
    private readonly IDistributedCache _cache;
    private readonly IChatService _chatService;
    private readonly ICommentService _commentService;
    private readonly IQuestionService _questionService;
    private readonly IAnswerService _answerService;
    private readonly IAnnouncementsService _announcementsService;
    private readonly WebPushService _webPushService;
    
    public MessageHub(
        IDistributedCache cache,
        IChatService chatService,
        ICommentService commentService,
        IAnswerService answerService,
        IQuestionService questionService,
        IAnnouncementsService announcementsService,
        WebPushService webPushService)
    {
        _cache = cache;
        _chatService = chatService;
        _commentService = commentService;
        _answerService = answerService;
        _questionService = questionService;
        _announcementsService = announcementsService;
        _webPushService = webPushService;
    }

    public async Task JoinChat(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
        var userId = Context.User.GetUserId();
        
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };

        await _cache.SetStringAsync($"user_conn_{userId}", JsonSerializer.Serialize(connection), options);
    }
    
    public async Task JoinChatGeneral(UserConnection connection)
    {
        var userId = Context.User.GetUserId();
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
        var stringConnection = JsonSerializer.Serialize(connection);
        
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };
        
        await _cache.SetStringAsync(Context.ConnectionId, stringConnection, options);
        await _cache.SetStringAsync($"active_question_room_{userId}", connection.ChatRoom,
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) });
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.GetUserId();
        await _cache.RemoveAsync(Context.ConnectionId);
        await _cache.RemoveAsync($"active_question_room_{userId}");
        await _cache.RemoveAsync($"user_conn_{userId}");
        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task SendMessage(Guid chatId, string message, string userName, Guid offerId)
    {
        var userId = Context.User.GetUserId();

        var connectionJson = await _cache.GetStringAsync($"user_conn_{userId}");
        if (connectionJson is null) return;

        var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        if (connection is null) return;

        var success = await _chatService.AddMessage(userId, chatId, message);
        if (!success) return;
        
        await Clients.Group(chatId.ToString())
            .ReceiveMessage(userId,
                connection.UserName,
                message,
                chatId);
        
        var participants = await _chatService.GetChatParticipants(chatId);
        
        var receiverIds = participants.Where(p => p != userId);
        
        foreach (var receiverId in receiverIds)
        {
            var activeRoom = await _cache.GetStringAsync($"user_conn_{receiverId}");

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

        if (success is not null)
            await Clients.Group(chatId.ToString()).ReceiveComment(success.Value, chatId, userName, message);
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
        
        await Clients.Group(chatId.ToString()).ReceiveQuestion(chatId, questionId.Value, userName, message);
        
        var authorId = await _announcementsService.GetAuthorOfferIdByQuestionId(questionId.Value);
        if (authorId == Guid.Empty)
            return;
        
        var activeRoom = await _cache.GetStringAsync($"active_question_room_{authorId}");

        if (activeRoom is not null)
            return;
        
        await _webPushService.SendNotificationToUserAsync(authorId, $"[{userName}] {message}", $"/offers/{chatId}/questions","New answer");
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
        
        await Clients.Group(chatId.ToString()).ReceiveAnswer(answerId.Value, questionId, userName, message);

        var questionUserId = await _questionService.GetQuestionUserIdByAnswerId(answerId.Value);
        if (questionUserId == Guid.Empty)
            return;
        
        var activeRoom = await _cache.GetStringAsync($"active_question_room_{questionUserId}");

        if (activeRoom is not null)
            return;
        
        await _webPushService.SendNotificationToUserAsync(questionUserId, $"[{userName}] {message}", $"/offers/{chatId}/questions", "New answer");
    }
    
    public async Task AddOffer(Guid chatId, AnnouncementShortDto offer)
    {
        var connectionJson = await _cache.GetStringAsync(Context.ConnectionId);
        if (connectionJson is null) return;
        var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        
        if (connection is null) return;

        await Clients.Group(chatId.ToString())
            .ReceiveOffer(offer);
        
        var participants = await _chatService.GetChatParticipants(chatId);
        
        foreach (var participantId in participants)
        {
            await Clients.Group(participantId.ToString())
                .UpdateOffer(offer);
        }
    }
    
    public async Task UpdateOffer(Guid chatId, AnnouncementShortDto offer)
    {
        var userId = Context.User.GetUserId();
        
        var connectionJson = await _cache.GetStringAsync(Context.ConnectionId);
        if (connectionJson is null) return;
        var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        
        if (connection is null) return;

        await Clients.Group(chatId.ToString())
            .UpdateOffer(offer);
        
        var participants = await _chatService.GetChatParticipants(chatId);
        
        foreach (var participantId in participants)
        {
            await Clients.Group(participantId.ToString())
                .UpdateOffer(offer);
        }
    }
    
    public async Task DeleteOffer(Guid chatId, Guid offerId)
    {
        var userId = Context.User.GetUserId();
        
        // var connectionJson = await _cache.GetStringAsync(Context.ConnectionId);
        // if (connectionJson is null) return;
        // var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        //
        // if (connection is null) return;

        await Clients.Group(chatId.ToString())
            .DeleteOffer(offerId);
        
        var participants = await _chatService.GetChatParticipants(chatId);
        
        foreach (var participantId in participants)
        {
            await Clients.Group(participantId.ToString())
                .DeleteOffer(offerId);
        }
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
            await Clients.Group(chatId.ToString()).DeleteComment(commentId);
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
            await Clients.Group(chatId.ToString()).DeleteAnswer(answerId);
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
            await Clients.Group(chatId.ToString()).DeleteQuestion(questionId);
    }
}