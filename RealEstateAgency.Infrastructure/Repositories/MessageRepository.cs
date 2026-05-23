using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class MessageRepository(RealEstateContext ctx) : IMessageRepository
{
    public async Task<Guid> AddMessageAsync(Message message)
    {
        await ctx.Messages.AddAsync(message);
        await ctx.SaveChangesAsync();
        return message.Id;
    }
    
    public async Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId)
    {
        var result = await ctx.Messages
            .AsNoTracking()
            .Where(x => x.ChatId == chatId)
            .OrderBy(x => x.CreatedAt)
            .Include(x => x.UserNavigation)
            .ToListAsync();
        return result;
    }
    
    public async Task<MessageGridDto?> GetByIdAsync(Guid messageId)
    {
        var result = await ctx.Messages
            .Where(x => x.Id == messageId)
            .Select(x => new MessageGridDto
            {
                Id = x.Id,
                Text = x.Content,
                CreatedAt = x.CreatedAt,
                UserFromLogin = x.UserNavigation!.UserName!,
                UserFromId = x.UserNavigation.Id,
                UserToId = x.ChatNavigation!.ChatMembersNavigation.Where(y => y.UserId != x.UserNavigation.Id).Select(y => y.UserNavigation!.Id).FirstOrDefault(),
                UserToLogin = x.ChatNavigation.ChatMembersNavigation.Where(y => y.UserId != x.UserNavigation.Id).Select(y => y.UserNavigation!.UserName).FirstOrDefault()!,
                ChatTypeId = x.ChatNavigation.TypeId
            })
            .FirstOrDefaultAsync();

        return result;
    }
    
    public async Task<List<MessageGridDto>> GetMessagesGridAsync()
    {
        var result = await ctx.Messages
            .Select(x => new MessageGridDto
            {
                Id = x.Id,
                Text = x.Content,
                CreatedAt = x.CreatedAt,
                UserFromLogin = x.UserNavigation!.UserName!,
                UserFromId = x.UserNavigation.Id,
                UserToId = x.ChatNavigation!.ChatMembersNavigation.Where(y => y.UserId != x.UserNavigation.Id).Select(y => y.UserNavigation!.Id).FirstOrDefault(),
                UserToLogin = x.ChatNavigation.ChatMembersNavigation.Where(y => y.UserId != x.UserNavigation.Id).Select(y => y.UserNavigation!.UserName).FirstOrDefault()!,
                ChatTypeId = x.ChatNavigation.TypeId
            })
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return result;
    }
}