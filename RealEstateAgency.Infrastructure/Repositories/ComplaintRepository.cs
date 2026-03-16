using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class ComplaintRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IComplaintRepository
{
    public async Task<bool> IsUserComplainedByUserIdAsync(Guid userId, Guid offerId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Complaints.AnyAsync(x  => x.UserId == userId && x.AnnouncementId  == offerId);
        return res;
    }
    
    public async Task<List<Complaint>> GetAllComplaintsAsync()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Complaints.ToListAsync();
        return res;
    }
    
    public async Task<List<Complaint>> GetAllOpenedComplaintsAsync()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Complaints.Where(x => x.ProcessedAt == null).ToListAsync();
        return res;
    }
    
    public async Task<Guid> InsertAsync(Complaint complaint)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Complaints.AddAsync(complaint);
        await ctx.SaveChangesAsync();
        return res.Entity.Id;
    }
    
    public async Task<bool> UpdateAsync(ComplaintDto complaint)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            var res = await ctx.Complaints.FindAsync(complaint.Id);

            if (res is null)
                return false;
        
            res.AdminId = complaint.AdminId;
            res.AdminNote = complaint.AdminNote;
            res.StatusId = complaint.StatusId;
            res.ProcessedAt = complaint.ProcessedAt;
        
            await ctx.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            await ctx.Complaints
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