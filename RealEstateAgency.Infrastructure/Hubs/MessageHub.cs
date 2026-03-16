using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Infrastructure.Hubs;

public interface IChatClient
{
    public Task ReceiveMessage(Guid userId, string userName, string message, Guid chatId);
    public Task ReceiveComment(Guid commentId, Guid offerId, string userName, string message);
    public Task ReceiveQuestion(Guid announcementId, Guid questionId, string userName, string message);
    public Task ReceiveAnswer(Guid answerId, Guid questionId, string userName, string message);
    public Task UpdateChatList(Guid chatId, Guid userId, string userName, string message);
    public Task ReceiveOffer(AnnouncementShort offer);
    public Task UpdateOffer(AnnouncementShort offer);
    public Task DeleteOffer(Guid offerId);
    public Task DeleteAnswer(Guid answerId);
    public Task DeleteQuestion(Guid questionId);
    public Task DeleteComment(Guid commentId);
}

public record UserConnection(string ChatRoom, string UserName);

//[Authorize]
public class MessageHub: Hub<IChatClient>
{
    private readonly IDistributedCache _cache;
    private readonly IChatService _chatService;
    private readonly ICommentsService _commentsService;
    private readonly IQuestionsService _questionsService;
    private readonly IAnswersService _answersService;
    private readonly IAnnouncementsService _announcementsService;
    
    public MessageHub(
        IDistributedCache cache,
        IChatService chatService,
        ICommentsService commentsService,
        IAnswersService answersService,
        IQuestionsService questionsService,
        IAnnouncementsService announcementsService)
    {
        _cache = cache;
        _chatService = chatService;
        _commentsService = commentsService;
        _answersService = answersService;
        _questionsService = questionsService;
        _announcementsService = announcementsService;
    }

    public async Task JoinChat(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

        var userId = Context.User.GetUserId();

        await _cache.SetStringAsync($"user_conn_{userId}", JsonSerializer.Serialize(connection));
    }
    
    public async Task JoinCommentsChat(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

        var stringConnection = JsonSerializer.Serialize(connection);
        
        await _cache.SetStringAsync(Context.ConnectionId, stringConnection);
    }
    
    public async Task JoinQuestionAnswerChat(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

        var stringConnection = JsonSerializer.Serialize(connection);
        
        await _cache.SetStringAsync(Context.ConnectionId, stringConnection);
    }
    
    public async Task JoinOffersChat(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

        var stringConnection = JsonSerializer.Serialize(connection);
        
        await _cache.SetStringAsync(Context.ConnectionId, stringConnection);
    }
    
    public async Task SendMessage(Guid chatId, string message)
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
        
        foreach (var participantId in participants)
        {
            await Clients.Group(participantId.ToString())
                .UpdateChatList(
                        chatId,
                        userId,
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
        
        var success = await _commentsService.InsertCommentAsync(objForAdding);

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
        
        var questionId = await _questionsService.InsertQuestionAsync(questionDto);
        
        if (questionId is not null)
            await Clients.Group(chatId.ToString()).ReceiveQuestion(chatId, questionId.Value, userName, message);
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
    
        var answerId = await _answersService.InsertAnswerAsync(answerDto);
        
        if (answerId is not null)
            await Clients.Group(chatId.ToString()).ReceiveAnswer(answerId.Value, questionId, userName, message);
    }
    
    public async Task AddOffer(Guid chatId, AnnouncementShort offer)
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
    
    public async Task UpdateOffer(Guid chatId, AnnouncementShort offer)
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
        
        var connectionJson = await _cache.GetStringAsync(Context.ConnectionId);
        if (connectionJson is null) return;
        var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        
        if (connection is null) return;

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
        
        var connectionJson = await _cache.GetStringAsync(Context.ConnectionId);
        if (connectionJson is null) return;
        var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        
        if (connection is null) return;


        var res = await _commentsService.DeleteByCommentIdAsync(commentId);
        
        if (res)
            await Clients.Group(chatId.ToString()).DeleteComment(commentId);
    }
    
    public async Task DeleteAnswer(Guid chatId, Guid answerId)
    {
        var userId = Context.User.GetUserId();
        
        var connectionJson = await _cache.GetStringAsync(Context.ConnectionId);
        if (connectionJson is null) return;
        var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        
        if (connection is null) return;


        var res = await _answersService.DeleteByAnswerIdAsync(answerId, userId);
        
        if (res)
            await Clients.Group(chatId.ToString()).DeleteAnswer(answerId);
    }
    
    public async Task DeleteQuestion(Guid chatId, Guid questionId)
    {
        var userId = Context.User.GetUserId();
        
        var connectionJson = await _cache.GetStringAsync(Context.ConnectionId);
        if (connectionJson is null) return;
        var connection = JsonSerializer.Deserialize<UserConnection>(connectionJson);
        
        if (connection is null) return;


        var res = await _questionsService.DeleteByQuestionIdAsync(questionId, userId);
        
        if (res)
            await Clients.Group(chatId.ToString()).DeleteQuestion(questionId);
    }
}