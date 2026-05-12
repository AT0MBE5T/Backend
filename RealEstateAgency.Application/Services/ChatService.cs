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
    IAuditService auditService,
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
            
            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.CreateChat),
                UserId = userId,
                Details = $"New chat created between {announcement.AuthorId} and {userId}"
            };
            
            await auditService.InsertAudit(auditDto);

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
        var global = await chatRepository.GetCommonChatAsync();
        if (global is not null)
            result.Insert(0, global);
        return result;
    }
    
    public async Task<List<MessageDto>> GetMessagesByChatId(Guid chatId)
    {
        var messages = await messageRepository.GetMessagesByChatIdAsync(chatId);
    
        var res = messages.Select(x => new MessageDto(
            x.Id,
            x.ChatId,
            x.SenderId,
            $"{x.UserNavigation?.Name ?? string.Empty} {x.UserNavigation?.Surname ?? string.Empty}",
            x.Content,
            x.CreatedAt,
            x.IsRead
        )).ToList();

        return res;
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
        
        var chatMembers = await chatRepository.GetMembersByChatIdAsync(chatId);

        if (!chatMembers.Select(x => x.UserId).Contains(userId) && chatId != new Guid("74679c97-aa14-444e-b3ae-9a6d8d01399f"))
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