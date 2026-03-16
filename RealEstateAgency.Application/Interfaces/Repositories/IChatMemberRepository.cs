using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IChatMemberRepository
{
    Task<bool> AddChatMemberAsync(ChatMember chatMember);
    Task<bool> RemoveChatMemberAsync(Guid userId, Guid chatId);
}