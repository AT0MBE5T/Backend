using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IChatMemberRepository
{
    Task<bool> AddChatMemberAsync(ChatMember chatMember);
}