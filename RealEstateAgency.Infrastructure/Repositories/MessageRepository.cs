using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class MessageRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IMessageRepository
{
    public async Task<bool> AddMessageAsync(Message message)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        await ctx.Messages.AddAsync(message);
        await ctx.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
    
        return await ctx.Messages
            .AsNoTracking()
            .Where(x => x.ChatId == chatId)
            .OrderBy(x => x.CreatedAt)
            .Include(x => x.UserNavigation)
            .ToListAsync();
    }
    
    public async Task<List<MessageGrid>> GetMessagesGridAsync()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
    
        var result = await ctx.Messages
            .Select(x => new MessageGrid
            {
                Id = x.Id,
                Text = x.Content,
                CreatedAt = x.CreatedAt,
                UserFromLogin = x.UserNavigation.UserName,
                UserFromId = x.UserNavigation.Id,
                UserToId = x.ChatNavigation.ChatMembersNavigation.Where(y => y.UserId != x.UserNavigation.Id).Select(y => y.UserNavigation.Id).FirstOrDefault(),
                UserToLogin = x.ChatNavigation.ChatMembersNavigation.Where(y => y.UserId != x.UserNavigation.Id).Select(y => y.UserNavigation.UserName).FirstOrDefault()
            })
            .ToListAsync();

        return result;
    }
    
    public async Task<bool> RemoveMessageAsync(Guid messageId)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            var res = await ctx.Messages
                .Where(m => m.Id == messageId)
                .ExecuteDeleteAsync();
            return res > 0;
        }
        catch
        {
            return false;
        }
    }
}