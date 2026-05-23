using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IMessageRepository
{
    Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId);
    Task<Guid> AddMessageAsync(Message message);
    Task<List<MessageGridDto>> GetMessagesGridAsync();
    Task<MessageGridDto?> GetByIdAsync(Guid messageId);
}