using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class ChatRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IChatRepository
{
    public async Task<List<ChatSummaryDto>> GetChatsByUserIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();

        var query = await ctx.ChatMembers
            .AsNoTracking()
            .Where(cm => cm.UserId == id)
            .Select(cm => new
            {
                ChatId = cm.ChatId,
                OtherMember = ctx.ChatMembers
                    .Where(other => other.ChatId == cm.ChatId && other.UserId != id)
                    .Select(other => other.UserNavigation)
                    .FirstOrDefault(),
                LastMessage = ctx.Messages
                    .Where(m => m.ChatId == cm.ChatId)
                    .OrderByDescending(m => m.CreatedAt)
                    .FirstOrDefault(),
                UnreadCount = ctx.Messages
                    .Count(m => m.ChatId == cm.ChatId && m.SenderId != id && !m.IsRead)
            })
            .ToListAsync();

        return query.Select(x => new ChatSummaryDto(
                x.ChatId,
                $"{x.OtherMember?.Name} {x.OtherMember?.Surname}",
                x.LastMessage?.Content ?? string.Empty,
                x.LastMessage?.CreatedAt, 
                x.UnreadCount,
                ""
            ))
            .OrderByDescending(x => x.LastMessageAt)
            .ToList();
    }
    
    public async Task<Guid?> GetChatByBothIdsAsync(Guid firstId, Guid secondId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Chats
            .Where(c => c.TypeId == Guid.Parse(ChatTypes.Private) &&
                        c.ChatMembersNavigation.Any(m => m.UserId == firstId) && 
                        c.ChatMembersNavigation.Any(m => m.UserId == secondId))
            .Select(c => (Guid?)c.Id)
            .FirstOrDefaultAsync();
    }
    
    public async Task<Chat?> GetChatByIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var result = await ctx.Chats
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        return result;
    }
    
    public async Task<List<ChatMember>> GetMembersByChatIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var result = await ctx.ChatMembers
            .AsNoTracking()
            .Where(x => x.ChatId == id)
            .ToListAsync();
        return result;
    }
    
    public async Task<Guid?> AddChatAsync(Chat chat)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            var res =  await ctx.Chats.AddAsync(chat);
            await ctx.SaveChangesAsync();
            return chat.Id;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteChatByIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Chats
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return res > 0;
    }
}