using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Services;

public class ChatService(
    IChatRepository chatRepository,
    IMessageRepository messageRepository,
    IChatMemberRepository chatMemberRepository,
    IUnitOfWork unitOfWork,
    IAnnouncementsService announcementsService): IChatService
{
    public async Task<Guid?> GetOrCreateChat(Guid userId, Guid announcementId)
    {
        var announcement = await announcementsService.GetAnnouncementFullById(announcementId, userId);

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
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }

        return chatId;
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

    public async Task<List<MessageGrid>> GetMessagesGrid()
    {
        var result = await  messageRepository.GetMessagesGridAsync();
        return result;
    }
}