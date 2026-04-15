using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IChatService
{
    Task<Guid?> GetOrCreateChat(Guid userId, Guid announcementId);
    Task<List<ChatSummaryDto>> GetChatsAsyncByUserId(Guid id);
    Task<List<MessageDto>> GetMessagesByChatId(Guid chatId);
    Task<bool> AddMessage(Guid userId, Guid chatId, string message);
    Task<List<Guid>> GetChatParticipants(Guid chatId);
    Task<List<MessageGrid>> GetMessagesGrid();
}