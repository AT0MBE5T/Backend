using Microsoft.Extensions.Logging;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Services;

public class ChatService(
    IChatRepository chatRepository,
    IMessageRepository messageRepository,
    IChatMemberRepository chatMemberRepository,
    IUnitOfWork unitOfWork,
    IAnnouncementsService announcementsService,
    ILogger<ChatService> logger): IChatService
{
    public async Task<Guid?> GetOrCreateChat(Guid userId, Guid announcementId)
    {
        var command = new AnnouncementInfoCommandDto(announcementId, userId);
        var announcement = await announcementsService.GetAnnouncementFullById(command);

        if (announcement is null || announcement.ClosedAt != null)
            return null;
        
        var chatId = await chatRepository.GetChatByBothIdsAsync(userId, announcement.AuthorId);

        if (chatId is not null)
            return chatId;
        
        var objToAdd = new Chat { TypeId = Guid.Parse(ChatTypes.Private), AnnouncementId = announcementId };

        try
        {
            await unitOfWork.BeginTransactionAsync();
        
            var addChatResult = await chatRepository.AddChatAsync(objToAdd);

            if (addChatResult is null)
                throw new Exception($"Could not create chat {objToAdd.Id}");
            
            chatId = objToAdd.Id;

            var chatMemberUser = new ChatMember
            {
                UserId = userId,
                ChatId = chatId.Value,
            };
        
            var chatMemberAuthor = new ChatMember
            {
                UserId = announcement.AuthorId,
                ChatId = chatId.Value,
            };
        
            var chatMemberResult1 = await chatMemberRepository.AddChatMemberAsync(chatMemberUser);
            
            if (!chatMemberResult1)
                throw new Exception("Could not create chat member");
            
            var chatMemberResult2 = await chatMemberRepository.AddChatMemberAsync(chatMemberAuthor);
            
            if (!chatMemberResult2)
                throw new Exception("Could not create chat member");

            await unitOfWork.CommitAsync();
        }
        catch(Exception ex)
        {
            logger.LogError("Failed to get or create chat: {ex}", ex);
            await unitOfWork.RollbackAsync();
            throw;
        }

        return chatId;
    }

    public async Task<bool> IsUserInThisChat(Guid userId, Guid chatId)
    {
        var participants = await GetChatParticipants(chatId);
        var result = participants.Contains(userId);
        return result;
    }

    public async Task<List<ChatSummaryDto>> GetChatsAsyncByUserId(Guid id)
    {
        var result = await chatRepository.GetChatsByUserIdAsync(id);
        return result;
    }
    
    public async Task<List<MessageDto>> GetMessagesByChatId(Guid chatId)
    {
        var messages = await messageRepository.GetMessagesByChatIdAsync(chatId);
    
        return messages.Select(x => new MessageDto(
            x.Id,
            x.ChatId,
            x.SenderId,
            x.UserNavigation?.Name ?? string.Empty,
            x.Content,
            x.CreatedAt,
            x.IsRead
        )).ToList();
    }
    
    public async Task<List<Guid>> GetChatParticipants(Guid chatId)
    {
        var chatMembers = await chatRepository.GetMembersByChatIdAsync(chatId);

        return chatMembers
            .Select(x => x.UserId)
            .ToList();
    }

    public async Task<bool> AddMessage(Guid userId, Guid chatId, string message)
    {
        var chat = await chatRepository.GetChatByIdAsync(chatId);

        if (chat is null)
            return false;
        
        var userChats = await chatRepository.GetChatsByUserIdAsync(userId);

        if (!userChats.Select(x => x.ChatId).Contains(chatId))
            return false;

        var newMessage = new Message
        {
            ChatId = chatId,
            Content = message,
            SenderId = userId,
            IsRead = false
        };
        
        var result = await messageRepository.AddMessageAsync(newMessage);

        return result;
    }

    public async Task<List<MessageGridDto>> GetMessagesGrid()
    {
        var result = await  messageRepository.GetMessagesGridAsync();
        return result;
    }
}