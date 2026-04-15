using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IMessageRepository
{
    Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId);
    Task<bool> AddMessageAsync(Message message);
    Task<bool> RemoveMessageAsync(Guid messageId);
    Task<List<MessageGrid>> GetMessagesGridAsync();
}