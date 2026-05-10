using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class SupportRepository (RealEstateContext ctx) : ISupportRepository
{
    public async Task<bool> IsUserHasUnclosedSupportAsync(Guid userId)
    {
        var res = await ctx.Supports
            .AnyAsync(x => x.UserId == userId && x.ClosedAt == null);
        return res;
    }
    
    public async Task<Support?> GetByIdAsync(Guid supportId)
    {
        var res = await ctx.Supports
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == supportId);
        return res;
    }
    
    public async Task<List<SupportGridDto>> GetAllSupportsAsync()
    {
        var res = await ctx.Supports
            .Select(x => new SupportGridDto
            {
                Id = x.Id,
                UserName = x.UserNavigation!.UserName!,
                AdminName = x.AdminNavigation!.UserName,
                CreatedAt = x.CreatedAt,
                ClosedAt = x.ClosedAt,
                UserNote = x.UserNote,
            }).ToListAsync();
        return res;
    }
    
    public async Task<List<SupportGridDto>> GetAllOpenedSupportsAsync()
    {
        var res = await ctx.Supports
            .Where(x => x.ClosedAt == null && x.AdminId == null)
            .Select(x => new SupportGridDto
            {
                Id = x.Id,
                UserName = x.UserNavigation!.UserName!,
                AdminName = x.AdminNavigation!.UserName,
                CreatedAt = x.CreatedAt,
                ClosedAt = x.ClosedAt,
                UserNote = x.UserNote,
            }).ToListAsync();
        return res;
    }
    
    public async Task<Guid> InsertAsync(Support support)
    {
        var res = await ctx.Supports.AddAsync(support);
        await ctx.SaveChangesAsync();
        return res.Entity.Id;
    }
    
    public async Task<bool> AdminJoinSupportAsync(Guid supportId, Guid adminId)
    {
        var res = await ctx.Supports.FindAsync(supportId);

        if (res is null)
            return false;

        res.AdminId = adminId;
    
        await ctx.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> CloseSupportAsync(Guid supportId)
    {
        var res = await ctx.Supports.FindAsync(supportId);

        if (res is null)
            return false;

        res.ClosedAt = DateTime.UtcNow;
    
        await ctx.SaveChangesAsync();
        return true;
    }
}