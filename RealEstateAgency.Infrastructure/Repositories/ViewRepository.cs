using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class ViewRepository(RealEstateContext ctx) : IViewRepository
{
    public async Task<bool> IsUserWatchedAsync(Guid userId, Guid announcementId)
    {
        var res = await ctx.Views.AnyAsync(v => v.UserId == userId && v.AnnouncementId == announcementId);
        return res;
    }
    
    public async Task<Guid> InsertAsync(View view)
    {
        var res = await ctx.Views.AddAsync(view);
        await ctx.SaveChangesAsync();
        return res.Entity.Id;
    }
}