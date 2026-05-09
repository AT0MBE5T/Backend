using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class ChatRepository(RealEstateContext ctx) : IChatRepository
{
    public async Task<List<ChatSummaryDto>> GetChatsByUserIdAsync(Guid id)
    {
        var query = await ctx.ChatMembers
            .AsNoTracking()
            .Where(cm => cm.UserId == id)
            .Select(cm => new
            {
                ChatId = cm.ChatId,
                ClosedAt = cm.ChatNavigation!.AnnouncementNavigation!.ClosedAt,
                OfferId = cm.ChatNavigation.AnnouncementNavigation.Id,
                RealtorId = cm.ChatNavigation.AnnouncementNavigation.StatementNavigation!.UserId,
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
                x.OtherMember?.Avatar,
                x.ClosedAt,
                x.OfferId,
                x.RealtorId,
                null
            ))
            .OrderByDescending(x => x.LastMessageAt)
            .ToList();
    }
    
    public async Task<ChatSummaryDto?> GetCommonChatAsync()
    {
        var query = await ctx.Chats
            .AsNoTracking()
            .Where(c => c.Id == new Guid("74679c97-aa14-444e-b3ae-9a6d8d01399f"))
            .Select(cm => new
            {
                LastMessage = ctx.Messages
                    .Where(m => m.ChatId == cm.Id)
                    .OrderByDescending(m => m.CreatedAt)
                    .Include(x => x.UserNavigation)
                    .FirstOrDefault()
            })
            .ToListAsync();
            
        var res = query.Select(x => new ChatSummaryDto(
                new Guid("74679c97-aa14-444e-b3ae-9a6d8d01399f"),
                "COMMON",
                x.LastMessage?.Content ?? string.Empty,
                x.LastMessage?.CreatedAt ?? DateTime.UtcNow,
                0,
                string.Empty,
                null,
                null,
                null,
                $"{x.LastMessage?.UserNavigation?.Name} {x.LastMessage?.UserNavigation?.Surname}"
            ))
            .FirstOrDefault();

        return res;
    }
    
    public async Task<Guid?> GetChatByBothIdsAsync(Guid firstId, Guid secondId)
    {
        var result = await ctx.Chats
            .AsNoTracking()
            .Where(c => c.TypeId == Guid.Parse(ChatTypes.Private) &&
                        c.ChatMembersNavigation.Any(m => m.UserId == firstId) && 
                        c.ChatMembersNavigation.Any(m => m.UserId == secondId))
            .Select(c => (Guid?)c.Id)
            .FirstOrDefaultAsync();
        return result;
    }
    
    public async Task<Chat?> GetChatByIdAsync(Guid id)
    {
        
        var result = await ctx.Chats
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        return result;
    }
    
    public async Task<List<ChatMember>> GetMembersByChatIdAsync(Guid id)
    {
        
        var result = await ctx.ChatMembers
            .AsNoTracking()
            .Where(x => x.ChatId == id)
            .ToListAsync();
        return result;
    }
    
    public async Task<Guid?> AddChatAsync(Chat chat)
    {
        var res = await ctx.Chats.AddAsync(chat);
        return chat.Id;
    }
}