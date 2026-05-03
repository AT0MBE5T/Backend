using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class ChatMemberRepository(RealEstateContext ctx) : IChatMemberRepository
{
    public async Task<bool> AddChatMemberAsync(ChatMember chatMember)
    {
        await ctx.ChatMembers.AddAsync(chatMember);
        return true;
    }
}