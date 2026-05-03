using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class ComplaintRepository(RealEstateContext ctx) : IComplaintRepository
{
    public async Task<bool> IsUserComplainedByUserIdAsync(Guid userId, Guid offerId)
    {
        var res = await ctx.Complaints
            .AnyAsync(x  => x.UserId == userId && x.AnnouncementId  == offerId);
        return res;
    }
    
    public async Task<Complaint?> GetByIdAsync(Guid complaintId)
    {
        var res = await ctx.Complaints
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == complaintId);
        return res;
    }
    
    public async Task<List<ComplaintGridDto>> GetComplaintsByUserId(Guid userId)
    {
        var res = await ctx.Complaints
            .Where(x  => x.UserId == userId)
            .Select(x => new ComplaintGridDto
            {
                Id = x.Id,
                AnnouncementId = x.AnnouncementId,
                AnnouncementName = x.AnnouncementNavigation!.StatementNavigation!.Title,
                TypeName = x.ComplaintTypeNavigation!.Name,
                UserName = x.UserNavigation!.UserName!,
                AdminName = x.AdminNavigation!.UserName,
                AdminNote = x.AdminNote,
                CreatedAt = x.CreatedAt,
                ProcessedAt = x.ProcessedAt,
                StatusName = x.ComplaintStatusNavigation!.Name,
                UserNote = x.UserNote,
            }).ToListAsync();
        return res;
    }
    
    public async Task<List<ComplaintGridDto>> GetAllComplaintsAsync()
    {
        var res = await ctx.Complaints
            .Select(x => new ComplaintGridDto
            {
                Id = x.Id,
                AnnouncementId = x.AnnouncementId,
                AnnouncementName = x.AnnouncementNavigation!.StatementNavigation!.Title,
                TypeName = x.ComplaintTypeNavigation!.Name,
                UserName = x.UserNavigation!.UserName!,
                AdminName = x.AdminNavigation!.UserName,
                AdminNote = x.AdminNote,
                CreatedAt = x.CreatedAt,
                ProcessedAt = x.ProcessedAt,
                StatusName = x.ComplaintStatusNavigation!.Name,
                UserNote = x.UserNote,
            }).ToListAsync();
        return res;
    }
    
    public async Task<List<ComplaintGridDto>> GetAllOpenedComplaintsAsync()
    {
        var res = await ctx.Complaints
            .Where(x => x.ProcessedAt == null)
            .Select(x => new ComplaintGridDto
            {
                Id = x.Id,
                AnnouncementId = x.AnnouncementId,
                AnnouncementName = x.AnnouncementNavigation!.StatementNavigation!.Title,
                TypeName = x.ComplaintTypeNavigation!.Name,
                UserName = x.UserNavigation!.UserName!,
                AdminName = x.AdminNavigation!.UserName,
                AdminNote = x.AdminNote,
                CreatedAt = x.CreatedAt,
                ProcessedAt = x.ProcessedAt,
                StatusName = x.ComplaintStatusNavigation!.Name,
                UserNote = x.UserNote,
            }).ToListAsync();
        return res;
    }
    
    public async Task<Guid> InsertAsync(Complaint complaint)
    {
        var res = await ctx.Complaints.AddAsync(complaint);
        await ctx.SaveChangesAsync();
        return res.Entity.Id;
    }
    
    public async Task<bool> UpdateAsync(Complaint complaint)
    {
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
}