using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.DTO;
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
    
    public async Task<Complaint?> GetByIdAsync(Guid complaintId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Complaints.FirstOrDefaultAsync(x => x.Id == complaintId);
        return res;
    }
    
    public async Task<List<ComplaintGrid>> GetComplaintsByUserId(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Complaints
            .Where(x  => x.UserId == userId)
            .Select(x => new ComplaintGrid
            {
                Id = x.Id,
                AnnouncementId = x.AnnouncementId,
                AnnouncementName = x.AnnouncementNavigation.StatementNavigation.Title,
                TypeName = x.ComplaintTypeNavigation.Name,
                UserName = x.UserNavigation.UserName,
                AdminName = x.AdminNavigation.UserName,
                AdminNote = x.AdminNote,
                CreatedAt = x.CreatedAt,
                ProcessedAt = x.ProcessedAt,
                StatusName = x.ComplaintStatusNavigation.Name,
                UserNote = x.UserNote,
            }).ToListAsync();
        return res;
    }
    
    public async Task<List<ComplaintGrid>> GetAllComplaintsAsync()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Complaints
            .Select(x => new ComplaintGrid
            {
                Id = x.Id,
                AnnouncementId = x.AnnouncementId,
                AnnouncementName = x.AnnouncementNavigation.StatementNavigation.Title,
                TypeName = x.ComplaintTypeNavigation.Name,
                UserName = x.UserNavigation.UserName,
                AdminName = x.AdminNavigation.UserName,
                AdminNote = x.AdminNote,
                CreatedAt = x.CreatedAt,
                ProcessedAt = x.ProcessedAt,
                StatusName = x.ComplaintStatusNavigation.Name,
                UserNote = x.UserNote,
            }).ToListAsync();
        return res;
    }
    
    public async Task<List<ComplaintGrid>> GetAllOpenedComplaintsAsync()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Complaints
            .Where(x => x.ProcessedAt == null)
            .Select(x => new ComplaintGrid
            {
                Id = x.Id,
                AnnouncementId = x.AnnouncementId,
                AnnouncementName = x.AnnouncementNavigation.StatementNavigation.Title,
                TypeName = x.ComplaintTypeNavigation.Name,
                UserName = x.UserNavigation.UserName,
                AdminName = x.AdminNavigation.UserName,
                AdminNote = x.AdminNote,
                CreatedAt = x.CreatedAt,
                ProcessedAt = x.ProcessedAt,
                StatusName = x.ComplaintStatusNavigation.Name,
                UserNote = x.UserNote,
            }).ToListAsync();
        return res;
    }
    
    public async Task<Guid> InsertAsync(Complaint complaint)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Complaints.AddAsync(complaint);
        await ctx.SaveChangesAsync();
        return res.Entity.Id;
    }
    
    public async Task<bool> UpdateAsync(Complaint complaint)
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