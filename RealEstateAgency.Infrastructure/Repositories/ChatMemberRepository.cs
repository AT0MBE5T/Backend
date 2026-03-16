using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class ChatMemberRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IChatMemberRepository
{
    public async Task<bool> AddChatMemberAsync(ChatMember chatMember)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            await ctx.ChatMembers.AddAsync(chatMember);
            await ctx.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<bool> RemoveChatMemberAsync(Guid userId, Guid chatId)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            await ctx.ChatMembers
                .Where(x => x.ChatId == chatId && x.UserId == userId)
                .ExecuteDeleteAsync();
            
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    // public async Task<bool> AddUserToChatAsync(Guid chatId, ChatMember chatMember)
    // {
    //     await using var ctx = await dbContextFactory.CreateDbContextAsync();
    //     var chat = await ctx.Chats.FirstOrDefaultAsync(x => x.Id == chatId);
    //     if (chat is null)
    //         return false;
    //     
    //     chat.ChatMembersNavigation.Add(chatMember);
    //     await ctx.SaveChangesAsync();
    //     return true;
    // }
    //
    // public async Task<bool> RemoveUserFromChatAsync(Guid chatId, Guid userId)
    // {
    //     try
    //     {
    //         await using var ctx = await dbContextFactory.CreateDbContextAsync();
    //         var chat = await ctx.Chats.FirstOrDefaultAsync(x => x.Id == chatId);
    //
    //         var member = chat?.ChatMembersNavigation.FirstOrDefault(x => x.UserId == userId);
    //
    //         if (member is null)
    //             return false;
    //         
    //         chat.ChatMembersNavigation.Remove(member);
    //         await ctx.SaveChangesAsync();
    //         return true;
    //     }
    //     catch
    //     {
    //         return false;
    //     }
    // }
}