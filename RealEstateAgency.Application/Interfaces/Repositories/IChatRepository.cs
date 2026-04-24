using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IChatRepository
{
    Task<List<ChatSummaryDto>> GetChatsByUserIdAsync(Guid id);
    Task<Guid?> AddChatAsync(Chat chat);
    Task<Chat?> GetChatByIdAsync(Guid id);
    Task<List<ChatMember>> GetMembersByChatIdAsync(Guid id);
    Task<Guid?> GetChatByBothIdsAsync(Guid firstId, Guid secondId);
}