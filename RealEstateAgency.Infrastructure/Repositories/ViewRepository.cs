using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class ViewRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IViewRepository
{
    public async Task<bool> IsUserWatchedAsync(Guid userId, Guid announcementId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Views.AnyAsync(v => v.UserId == userId && v.AnnouncementId == announcementId);
        return res;
    }
    
    public async Task<int> GetCntByUserIdAsync(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Views.CountAsync(v => v.UserId == userId);
        return res;
    }
    
    public async Task<int> GetCntByOfferIdAsync(Guid announcementId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Views.CountAsync(v => v.AnnouncementId == announcementId);
        return res;
    }
    
    public async Task<View?> GetDataByUserOfferId(Guid userId, Guid offerId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Views.FirstOrDefaultAsync(v => v.AnnouncementId == offerId && v.UserId == userId);
        return res;
    }
    
    public async Task<Guid> InsertAsync(View view)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Views.AddAsync(view);
        await ctx.SaveChangesAsync();
        return res.Entity.Id;
    }
    
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            await ctx.Views
                .Where(v => v.Id == id)
                .ExecuteDeleteAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}