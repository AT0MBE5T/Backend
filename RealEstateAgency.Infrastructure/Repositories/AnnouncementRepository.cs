using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class AnnouncementRepository(RealEstateContext ctx) : IAnnouncementRepository
{
    public async Task<Guid> InsertAsync(Announcement announcement)
    {
        await ctx.Announcements.AddAsync(announcement);
        return announcement.Id;
    }
    
    public async Task<bool> UpdateAsync(Guid id, Announcement announcement)
    {
        try
        {
            var announcementToUpdate = await ctx.Announcements.FindAsync(id);
            if (announcementToUpdate == null) return false;
            announcementToUpdate.StatementId = announcement.StatementId;
            announcementToUpdate.UpdatedAt = DateTime.UtcNow;
            announcementToUpdate.UpdatedBy = announcement.StatementNavigation?.UserId;
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var entity = await ctx.Announcements.FindAsync(id);
            if (entity is null)
                return false;
            
            ctx.Announcements.Remove(entity);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<Announcement?> GetAnnouncementById(Guid id)
    {
        var result = await ctx.Announcements
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return result;
    }
    
    public async Task<List<AnnouncementGridDto>> GetAnnouncementsGridAsync()
    {
        
        var result = await ctx.Announcements
            .Select(x => new AnnouncementGridDto
            {
                Id = x.Id,
                CreatedAt = x.StatementNavigation!.CreatedAt,
                Title = x.StatementNavigation.Title,
                Author = x.StatementNavigation.UserNavigation!.UserName!,
                PropertyTypeName = x.StatementNavigation.PropertyNavigation!.PropertyTypeNavigation!.Name,
                StatementTypeName = x.StatementNavigation.StatementTypeNavigation!.Name,
                AuthorId = x.StatementNavigation.UserNavigation.Id,
                ClosedAt = x.ClosedAt,
                IsVerified = x.VerificationNavigation != null,
                Price = x.StatementNavigation.Price,
                PropertyTypeId = x.StatementNavigation.PropertyNavigation.PropertyTypeId,
                StatementTypeId = x.StatementNavigation.StatementTypeId,
                ViewsCnt = x.ViewsNavigation.Count
            })
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
        return result;
    }
    
    public async Task<Verification?> GetVerificationAsync(Guid id)
    {
        var result = await ctx.Verifications
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.AnnouncementId == id);
        
        return result;
    }
    
    public async Task<AnnouncementFullDto?> GetAnnouncementFullById(Guid id, Guid? userId)
    {
        
        return await ctx.Announcements
            .Where(x => x.Id == id)
            .Select(x => new AnnouncementFullDto
            {
                Id = x.Id,
                Area = x.StatementNavigation!.PropertyNavigation!.Area,
                Author = x.StatementNavigation.UserNavigation!.Name + ' ' + x.StatementNavigation.UserNavigation.Surname,
                AuthorId = x.StatementNavigation.UserId,
                Content = x.StatementNavigation.Content,
                CreatedAt = x.PublishedAt,
                Description = x.StatementNavigation.PropertyNavigation.Description,
                Floors = x.StatementNavigation.PropertyNavigation.Floors,
                Location = x.StatementNavigation.PropertyNavigation.Location,
                Photos = x.StatementNavigation.PropertyNavigation.ImagesNavigation.OrderBy(i => i.OrderIndex).Select(i => new PhotoDto { Id = i.Id, Url = i.PhotoUrl }).ToList(),
                Price = x.StatementNavigation.Price,
                Rooms = x.StatementNavigation.PropertyNavigation.Rooms,
                Title = x.StatementNavigation.Title,
                PropertyTypeName = x.StatementNavigation.PropertyNavigation.PropertyTypeNavigation!.Name,
                StatementTypeName = x.StatementNavigation.StatementTypeNavigation!.Name,
                IsVerified = x.VerificationNavigation != null,
                IsFavorite = ctx.Favorites
                    .Any(f => f.AnnouncementId == x.Id && f.UserId == userId),
                ViewsCnt = ctx.Views.Count(y => y.AnnouncementId == id),
                ClosedAt = x.ClosedAt
            })
            .FirstOrDefaultAsync();
    }

    private static IQueryable<Announcement> GetTextSearchQuery(IQueryable<Announcement> query, string text)
    {
        return query
            .Where(x => x.StatementNavigation!.Title.Contains(text) || x.StatementNavigation.PropertyNavigation!.Location.Contains(text));
    }
    
    private IQueryable<Announcement> GetFilteredSearchQuery(RealEstateContext ctx, IQueryable<Announcement> query, List<string> filtersId)
    {
        var propertyTypeIds = filtersId
            .Select(Guid.Parse)
            .Where(id => ctx.PropertyTypes.Any(pt => pt.Id == id))
            .ToList();

        var statementTypeIds = filtersId
            .Select(Guid.Parse)
            .Where(id => ctx.StatementTypes.Any(st => st.Id == id))
            .ToList();

        query = query.Where(x =>
            propertyTypeIds.Contains(x.StatementNavigation!.PropertyNavigation!.PropertyTypeId) ||
            statementTypeIds.Contains(x.StatementNavigation.StatementTypeId));
        
        return query;
    }
    
    private IQueryable<Announcement> GetSortedSearchQuery(IQueryable<Announcement> query, int sortId)
    {
        return sortId switch
        {
            1 => query.OrderByDescending(x => x.StatementNavigation!.Price),
            2 => query.OrderByDescending(x => x.StatementNavigation!.PropertyNavigation!.Area),
            3 => query.OrderByDescending(x => x.StatementNavigation!.PropertyNavigation!.Rooms),
            4 => query.OrderByDescending(x => x.StatementNavigation!.PropertyNavigation!.Floors),
            5 => query.OrderByDescending(x => x.ViewsNavigation.Count),
            6 => query.OrderByDescending(x => x.PublishedAt),
            7 => query.OrderBy(x => x.StatementNavigation!.Price),
            8 => query.OrderBy(x => x.StatementNavigation!.PropertyNavigation!.Area),
            9 => query.OrderBy(x => x.StatementNavigation!.PropertyNavigation!.Rooms),
            10 => query.OrderBy(x => x.StatementNavigation!.PropertyNavigation!.Floors),
            11 => query.OrderBy(x => x.ViewsNavigation.Count),
            12 => query.OrderBy(x => x.PublishedAt),
            _ => query
        };
    }

    private IQueryable<Announcement> GetBaseSortedSearchQuery(IQueryable<Announcement> query)
    {
        var res = query.OrderByDescending(a => a.VerificationNavigation != null)
            .ThenByDescending(a => a.PublishedAt)
            .ThenByDescending(a => a.ViewsNavigation.Count);

        return res;
    }
    
    public async Task<AnnouncementsShortAndPagesDto> GetSearchData(string text, List<string> filtersId, int sortId, int pageNumber, int pageSize, Guid userId)
    {
        
        var query = ctx.Announcements
            .AsNoTracking()
            .Where(x => x.ClosedAt == null);

        if (!string.IsNullOrEmpty(text))
        {
            query = GetTextSearchQuery(query, text);
        }

        if (filtersId.Count > 0)
        {
            query = GetFilteredSearchQuery(ctx, query, filtersId);
        }

        if (sortId > 0)
        {
            query = GetSortedSearchQuery(query, sortId);
        }

        query = GetBaseSortedSearchQuery(query);
        
        var pagesCnt = Math.Ceiling((double)query.Count() / pageSize);
        var totalItems = await query.CountAsync();
        
        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AnnouncementShortDto
            {
                Id = x.Id,
                Title = x.StatementNavigation!.Title,
                StatementTypeName = x.StatementNavigation.StatementTypeNavigation!.Name,
                Price = x.StatementNavigation.Price,
                Area = x.StatementNavigation.PropertyNavigation!.Area,
                Location = x.StatementNavigation.PropertyNavigation.Location,
                PhotoUrl = x.StatementNavigation.PropertyNavigation.ImagesNavigation.OrderBy(i => i.OrderIndex).FirstOrDefault()!.PhotoUrl,
                PropertyTypeName = x.StatementNavigation.PropertyNavigation.PropertyTypeNavigation!.Name,
                IsVerified = x.VerificationNavigation != null,
                IsFavorite = ctx.Favorites
                    .Any(f => f.AnnouncementId == x.Id && f.UserId == userId),
                ViewsCnt = ctx.Views.Count(v => v.AnnouncementId == x.Id),
                ClosedAt = x.ClosedAt
            }).ToListAsync();

        return new AnnouncementsShortAndPagesDto
        {
            Data = data,
            PagesCnt = (int)pagesCnt,
            TotalItems = totalItems
        };
    }

    public async Task<Guid> GetAuthorOfferIdByQuestionIdAsync(Guid id)
    {
        
        var result = await ctx.Questions
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => x.AnnouncementNavigation!.StatementNavigation!.UserId)
            .FirstOrDefaultAsync();
        return result;
    }
    
    public async Task<AnnouncementShortDto?> GetAnnouncementShortByOfferId(Guid offerId, Guid userId)
    {
        
        var data = await ctx.Announcements
            .Where(x => x.ClosedAt == null && x.Id == offerId)
            .Select(x => new AnnouncementShortDto
            {
                Id = x.Id,
                Title = x.StatementNavigation!.Title,
                StatementTypeName = x.StatementNavigation.StatementTypeNavigation!.Name,
                Price = x.StatementNavigation.Price,
                Area = x.StatementNavigation.PropertyNavigation!.Area,
                Location = x.StatementNavigation.PropertyNavigation.Location,
                PhotoUrl = x.StatementNavigation.PropertyNavigation.ImagesNavigation.OrderBy(i => i.OrderIndex).FirstOrDefault()!.PhotoUrl,
                PropertyTypeName = x.StatementNavigation.PropertyNavigation.PropertyTypeNavigation!.Name,
                IsVerified = x.VerificationNavigation != null,
                IsFavorite = ctx.Favorites
                    .Any(f => f.AnnouncementId == x.Id && f.UserId == userId),
                ViewsCnt = ctx.Views.Count(v => v.AnnouncementId == x.Id),
                ClosedAt = x.ClosedAt
            }).FirstOrDefaultAsync();

        return data;
    }

    public async Task<bool> SetClosedAt(Guid id)
    {
        
        var announcement = await ctx.Announcements.FindAsync(id);

        if (announcement == null)
        {
            return false;
        }
        
        announcement.ClosedAt = DateTime.UtcNow;
        await ctx.SaveChangesAsync();
        return true;
    }
    
    public async Task<AnnouncementsShortAndPagesDto> GetPlacedByUserId(Guid userId, int pageNumber, int pageSize)
    {
        var query = ctx.Announcements
            .AsNoTracking()
            .Where(x => x.StatementNavigation!.UserId == userId && x.ClosedAt == null);
        
        var pagesCnt = Math.Ceiling((double)query.Count() / pageSize);
        var totalItems = await query.CountAsync();
        
        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AnnouncementShortDto
            {
                Id = x.Id,
                Title = x.StatementNavigation!.Title,
                StatementTypeName = x.StatementNavigation.StatementTypeNavigation!.Name,
                Price = x.StatementNavigation.Price,
                Area = x.StatementNavigation.PropertyNavigation!.Area,
                Location = x.StatementNavigation.PropertyNavigation.Location,
                PhotoUrl = x.StatementNavigation.PropertyNavigation.ImagesNavigation.FirstOrDefault()!.PhotoUrl,
                PropertyTypeName = x.StatementNavigation.PropertyNavigation.PropertyTypeNavigation!.Name,
                IsVerified = x.VerificationNavigation != null,
                IsFavorite = ctx.Favorites
                    .Any(f => f.AnnouncementId == x.Id && f.UserId == userId),
                ViewsCnt = ctx.Views.Count(v => v.AnnouncementId == x.Id),
                ClosedAt = x.ClosedAt
            }).ToListAsync();

        return new AnnouncementsShortAndPagesDto
        {
            Data = data,
            PagesCnt = (int)pagesCnt,
            TotalItems = totalItems
        };
    }
    
    public async Task<AnnouncementsShortAndPagesDto> GetSoldByUserId(Guid userId, int pageNumber, int pageSize)
    {
        
        var query = ctx.Announcements
            .AsNoTracking()
            .Where(x => x.StatementNavigation!.UserId == userId && x.ClosedAt != null);
        
        var pagesCnt = Math.Ceiling((double)query.Count() / pageSize);
        var totalItems = await query.CountAsync();
        
        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AnnouncementShortDto
            {
                Id = x.Id,
                Title = x.StatementNavigation!.Title,
                StatementTypeName = x.StatementNavigation.StatementTypeNavigation!.Name,
                Price = x.StatementNavigation.Price,
                Area = x.StatementNavigation.PropertyNavigation!.Area,
                Location = x.StatementNavigation.PropertyNavigation.Location,
                PhotoUrl = x.StatementNavigation.PropertyNavigation.ImagesNavigation.FirstOrDefault()!.PhotoUrl,
                PropertyTypeName = x.StatementNavigation.PropertyNavigation.PropertyTypeNavigation!.Name,
                IsVerified = x.VerificationNavigation != null,
                IsFavorite = ctx.Favorites
                    .Any(f => f.AnnouncementId == x.Id && f.UserId == userId),
                ViewsCnt = ctx.Views.Count(v => v.AnnouncementId == x.Id),
                ClosedAt = x.ClosedAt
            }).ToListAsync();

        return new AnnouncementsShortAndPagesDto
        {
            Data = data,
            PagesCnt = (int)pagesCnt,
            TotalItems = totalItems
        };
    }
    
    public async Task<AnnouncementsShortAndPagesDto> GetBoughtByUserId(Guid userId, int pageNumber, int pageSize)
    {
        
        var query = ctx.Announcements
            .AsNoTracking()
            .Where(x => x.PaymentNavigation!.CustomerId == userId && x.ClosedAt != null);
        var pagesCnt = Math.Ceiling((double)query.Count() / pageSize);
        var totalItems = await query.CountAsync();
        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AnnouncementShortDto
            {
                Id = x.Id,
                Title = x.StatementNavigation!.Title,
                StatementTypeName = x.StatementNavigation.StatementTypeNavigation!.Name,
                Price = x.StatementNavigation.Price,
                Area = x.StatementNavigation.PropertyNavigation!.Area,
                Location = x.StatementNavigation.PropertyNavigation.Location,
                PhotoUrl = x.StatementNavigation.PropertyNavigation.ImagesNavigation.FirstOrDefault()!.PhotoUrl,
                PropertyTypeName = x.StatementNavigation.PropertyNavigation.PropertyTypeNavigation!.Name,
                IsVerified = x.VerificationNavigation != null,
                IsFavorite = ctx.Favorites
                    .Any(f => f.AnnouncementId == x.Id && f.UserId == userId),
                ViewsCnt = ctx.Views.Count(v => v.AnnouncementId == x.Id),
                ClosedAt = x.ClosedAt
            }).ToListAsync();

        return new AnnouncementsShortAndPagesDto
        {
            Data = data,
            PagesCnt = (int)pagesCnt,
            TotalItems = totalItems
        };
    }

    public async Task<int> GetTotalAnnouncements()
    {
        
        return await ctx.Announcements
            .Where(x => x.ClosedAt != null)
            .CountAsync();
    }
    
    public async Task<int> GetTotalViews()
    {
        
        return await ctx.Views
            .CountAsync();
    }
    
    public async Task<decimal> GetTotalIncome()
    {
        
        return await ctx.Announcements
            .Where(x => x.ClosedAt != null)
            .Select(x => x.StatementNavigation!.Price)
            .SumAsync();
    }
    
    public async Task<GeneralTopDealDto?> GetTopDeal()
    {
        
        return await ctx.Announcements
            .Where(x => x.ClosedAt != null)
            .OrderByDescending(x => x.StatementNavigation!.Price)
            .Select(x => new GeneralTopDealDto
            {
                TopDealName = x.StatementNavigation!.Title,
                TopDealStatementType = x.StatementNavigation.StatementTypeNavigation!.Name,
                TopDealCustomerName = x.PaymentNavigation!.CustomerNavigation!.Name,
                TopDealSoldDate = x.ClosedAt!.Value.Date,
                TopDealRealtorName = x.StatementNavigation.UserNavigation!.Name + ' ' + x.StatementNavigation.UserNavigation.Surname,
                TopDealPrice = x.StatementNavigation.Price,
            })
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<GeneralTopRealtorsDto>> GetTopRealtors(int top)
    {
        var result = await ctx.Announcements
            .Where(a => a.ClosedAt != null)
            .GroupBy(a => new { a.StatementNavigation!.UserId, a.StatementNavigation.UserNavigation!.Name,  a.StatementNavigation.UserNavigation.Surname})
            .Select(g => new GeneralTopRealtorsDto
            {
                TopRealtorName = g.Key.Name + ' ' + g.Key.Surname,
                TopRealtorDeals = g.Count(),
                TopRealtorIncome = g.Sum(x => x.StatementNavigation!.Price),
            })
            .OrderByDescending(x => x.TopRealtorIncome)
            .Take(top)
            .ToListAsync();
        return result;
    }
    
    public async Task<List<GeneralTopPropertyDto>> GetTopPropertyTypes(int top)
    {
        var result = await ctx.Announcements
            .Where(a => a.ClosedAt != null)
            .GroupBy(a => new { a.StatementNavigation!.PropertyNavigation!.PropertyTypeId, a.StatementNavigation.PropertyNavigation.PropertyTypeNavigation!.Name})
            .Select(g => new GeneralTopPropertyDto
            {
                TopPropertyTypeName = g.Key.Name,
                TopPropertyTypeCnt = g.Count(),
                TopPropertyTypeAvgPrice = g.Average(x => x.StatementNavigation!.Price),
            })
            .OrderByDescending(x => x.TopPropertyTypeAvgPrice)
            .Take(top)
            .ToListAsync();
        return result;
    }
    
    public async Task<List<GeneralTopClientDto>> GetTopClients(int top)
    {
        var result = await ctx.Announcements
            .Where(a => a.ClosedAt != null)
            .GroupBy(a => new { a.PaymentNavigation!.CustomerId, a.PaymentNavigation.CustomerNavigation!.Name, a.PaymentNavigation.CustomerNavigation.Surname})
            .Select(g => new GeneralTopClientDto
            {
                TopClientName = g.Key.Name +  ' ' + g.Key.Surname,
                TopClientDeals = g.Count(),
                TopClientSpent = g.Sum(x => x.StatementNavigation!.Price),
            })
            .OrderByDescending(x => x.TopClientSpent)
            .Take(top)
            .ToListAsync();
        return result;
    }
    
    public async Task<Guid?> GetPropertyIdByAnnouncementIdAsync(Guid announcementId)
    {
        var announcement = await ctx.Announcements
            .AsNoTracking()
            .Include(announcement => announcement.StatementNavigation)
            .FirstOrDefaultAsync(x => x.Id == announcementId);
        
        return announcement?.StatementNavigation?.PropertyId;
    }
    
    public async Task<Guid?> GetStatementIdByAnnouncementIdAsync(Guid announcementId)
    {
        var announcement = await ctx.Announcements
            .AsNoTracking()
            .Where(x => x.Id == announcementId)
            .FirstOrDefaultAsync();
        
        return announcement?.StatementId;
    }
}