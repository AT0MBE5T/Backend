using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IChatService
{
    Task<Guid?> GetOrCreateChat(Guid userId, Guid announcementId);
    Task<List<ChatSummaryDto>> GetChatsAsyncByUserId(Guid id);
    Task<List<MessageDto>> GetMessagesByChatId(Guid chatId);
    Task<bool> IsUserInThisChat(Guid userId, Guid chatId);
    Task<bool> AddMessage(Guid userId, Guid chatId, string message);
    Task<List<Guid>> GetChatParticipants(Guid chatId);
    Task<List<MessageGridDto>> GetMessagesGrid();
}