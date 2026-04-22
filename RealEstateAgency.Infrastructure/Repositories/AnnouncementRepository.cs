using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RealEstateAgency.API.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class AnnouncementRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IAnnouncementRepository
{
    public async Task<List<Announcement>> GetAllPaginatedAsync(int pageNumber, int pageSize)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();

        if (!await ctx.Announcements.AnyAsync())
        {
            return [];
        }
        
        return await ctx.Announcements
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Guid> InsertAsync(Announcement announcement)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        await ctx.Announcements.AddAsync(announcement);
        await ctx.SaveChangesAsync();
        return announcement.Id;
    }
    
    public async Task<bool> UpdateAsync(Guid id, Announcement announcement)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        try
        {
            var announcementToUpdate = await ctx.Announcements.FindAsync(id);
            if (announcementToUpdate != null)
            {
                announcementToUpdate.StatementId = announcement.StatementId;
                announcementToUpdate.UpdatedAt = DateTime.UtcNow;
                announcementToUpdate.UpdatedBy = announcement.StatementNavigation?.UserId;
            }
            await ctx.SaveChangesAsync();
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
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            await ctx.Announcements
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();
            
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<Announcement?> GetAnnouncementById(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Announcements
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<AnnouncementGrid>> GetAnnouncementsGridAsync()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var result = await ctx.Announcements
            .Select(x => new AnnouncementGrid
            {
                Id = x.Id,
                CreatedAt = x.StatementNavigation.CreatedAt,
                Title = x.StatementNavigation.Title,
                Author = x.StatementNavigation.UserNavigation.UserName,
                PropertyTypeName = x.StatementNavigation.PropertyNavigation.PropertyTypeNavigation.Name,
                StatementTypeName = x.StatementNavigation.StatementTypeNavigation.Name,
                AuthorId = x.StatementNavigation.UserNavigation.Id,
                ClosedAt = x.ClosedAt,
                IsVerified = x.VerificationNavigation != null,
                Price = x.StatementNavigation.Price,
                PropertyTypeId = x.StatementNavigation.PropertyNavigation.PropertyTypeId,
                StatementTypeId = x.StatementNavigation.StatementTypeId,
                ViewsCnt = x.ViewsNavigation.Count
            })
            .ToListAsync();
        return result;
    }
    
    public async Task<Verification?> GetVerificationAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Announcements
            .Where(x => x.Id == id)
            .Select(x => x.VerificationNavigation)
            .FirstOrDefaultAsync();
    }
    
    public async Task<AnnouncementFull?> GetAnnouncementFullById(Guid id, Guid? userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Announcements
            .Where(x => x.Id == id)
            .Select(x => new AnnouncementFull
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
                //Photo = $"data:image/png;base64,{Convert.ToBase64String(x.StatementNavigation.PropertyNavigation.ImageNavigation!.Bytes)}",
                Price = x.StatementNavigation.Price,
                Rooms = x.StatementNavigation.PropertyNavigation.Rooms,
                Title = x.StatementNavigation.Title,
                PropertyTypeName = x.StatementNavigation.PropertyNavigation.PropertyTypeNavigation!.Name,
                StatementTypeName = x.StatementNavigation.StatementTypeNavigation!.Name,
                IsVerified = x.VerificationNavigation != null,
                IsFavorite = ctx.Favorites
                    .Any(f => f.AnnouncementId == x.Id && f.UserId == userId),
                ViewsCnt = ctx.Views.Count(x => x.AnnouncementId == id),
                ClosedAt = x.ClosedAt
            })
            .FirstOrDefaultAsync();
    }

    private IQueryable<Announcement> GetTextSearchQuery(IQueryable<Announcement> query, string text)
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
    
    public async Task<AnnouncementsShortAndPages> GetSearchData(string text, List<string> filtersId, int sortId, int pageNumber, int pageSize, Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var query = ctx.Announcements.Where(x => x.ClosedAt == null);

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
            .Select(x => new AnnouncementShort
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

        return new AnnouncementsShortAndPages
        {
            Data = data,
            PagesCnt = (int)pagesCnt,
            TotalItems = totalItems
        };
    }

    public async Task<Guid> GetAuthorOfferIdByQuestionIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var result = await ctx.Questions
            .Where(x => x.Id == id)
            .Select(x => x.AnnouncementNavigation.StatementNavigation.UserId)
            .FirstOrDefaultAsync();
        return result;
    }
    
    public async Task<AnnouncementShort?> GetAnnouncementShortByOfferId(Guid offerId, Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var data = await ctx.Announcements
            .Where(x => x.ClosedAt == null && x.Id == offerId)
            .Select(x => new AnnouncementShort
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
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var announcement = await ctx.Announcements.FindAsync(id);

        if (announcement == null)
        {
            return false;
        }
        
        announcement.ClosedAt = DateTime.UtcNow;
        await ctx.SaveChangesAsync();
        return true;
    }
    
    public async Task<AnnouncementsShortAndPages> GetPlacedByUserId(Guid userId, int pageNumber, int pageSize)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var query = ctx.Announcements.Where(x => x.StatementNavigation!.UserId == userId && x.ClosedAt == null);
            

        var pagesCnt = Math.Ceiling((double)query.Count() / pageSize);
        var totalItems = await query.CountAsync();
        
        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AnnouncementShort
            {
                Id = x.Id,
                Title = x.StatementNavigation!.Title,
                StatementTypeName = x.StatementNavigation.StatementTypeNavigation!.Name,
                Price = x.StatementNavigation.Price,
                Area = x.StatementNavigation.PropertyNavigation!.Area,
                Location = x.StatementNavigation.PropertyNavigation.Location,
                //Photo = x.StatementNavigation.PropertyNavigation.ImageNavigation!.Bytes,
                PhotoUrl = x.StatementNavigation.PropertyNavigation.ImagesNavigation.FirstOrDefault().PhotoUrl,
                PropertyTypeName = x.StatementNavigation.PropertyNavigation.PropertyTypeNavigation!.Name,
                IsVerified = x.VerificationNavigation != null,
                IsFavorite = ctx.Favorites
                    .Any(f => f.AnnouncementId == x.Id && f.UserId == userId),
                ViewsCnt = ctx.Views.Count(v => v.AnnouncementId == x.Id),
                ClosedAt = x.ClosedAt
            }).ToListAsync();

        return new AnnouncementsShortAndPages
        {
            Data = data,
            PagesCnt = (int)pagesCnt,
            TotalItems = totalItems
        };
    }
    
    public async Task<AnnouncementsShortAndPages> GetSoldByUserId(Guid userId, int pageNumber, int pageSize)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var query = ctx.Announcements.Where(x => x.StatementNavigation!.UserId == userId && x.ClosedAt != null);
        
        var pagesCnt = Math.Ceiling((double)query.Count() / pageSize);
        var totalItems = await query.CountAsync();
        
        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AnnouncementShort
            {
                Id = x.Id,
                Title = x.StatementNavigation!.Title,
                StatementTypeName = x.StatementNavigation.StatementTypeNavigation!.Name,
                Price = x.StatementNavigation.Price,
                Area = x.StatementNavigation.PropertyNavigation!.Area,
                Location = x.StatementNavigation.PropertyNavigation.Location,
                //Photo = x.StatementNavigation.PropertyNavigation.ImageNavigation!.Bytes,
                PhotoUrl = x.StatementNavigation.PropertyNavigation.ImagesNavigation.FirstOrDefault().PhotoUrl,
                PropertyTypeName = x.StatementNavigation.PropertyNavigation.PropertyTypeNavigation!.Name,
                IsVerified = x.VerificationNavigation != null,
                IsFavorite = ctx.Favorites
                    .Any(f => f.AnnouncementId == x.Id && f.UserId == userId),
                ViewsCnt = ctx.Views.Count(v => v.AnnouncementId == x.Id),
                ClosedAt = x.ClosedAt
            }).ToListAsync();

        return new AnnouncementsShortAndPages
        {
            Data = data,
            PagesCnt = (int)pagesCnt,
            TotalItems = totalItems
        };
    }
    
    public async Task<AnnouncementsShortAndPages> GetBoughtByUserId(Guid userId, int pageNumber, int pageSize)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var query = ctx.Announcements.Where(x => x.PaymentNavigation!.CustomerId == userId && x.ClosedAt != null);
        var pagesCnt = Math.Ceiling((double)query.Count() / pageSize);
        var totalItems = await query.CountAsync();
        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AnnouncementShort
            {
                Id = x.Id,
                Title = x.StatementNavigation!.Title,
                StatementTypeName = x.StatementNavigation.StatementTypeNavigation!.Name,
                Price = x.StatementNavigation.Price,
                Area = x.StatementNavigation.PropertyNavigation!.Area,
                Location = x.StatementNavigation.PropertyNavigation.Location,
                //Photo = x.StatementNavigation.PropertyNavigation.ImageNavigation!.Bytes,
                PhotoUrl = x.StatementNavigation.PropertyNavigation.ImagesNavigation.FirstOrDefault().PhotoUrl,
                PropertyTypeName = x.StatementNavigation.PropertyNavigation.PropertyTypeNavigation!.Name,
                IsVerified = x.VerificationNavigation != null,
                IsFavorite = ctx.Favorites
                    .Any(f => f.AnnouncementId == x.Id && f.UserId == userId),
                ViewsCnt = ctx.Views.Count(v => v.AnnouncementId == x.Id),
                ClosedAt = x.ClosedAt
            }).ToListAsync();

        return new AnnouncementsShortAndPages
        {
            Data = data,
            PagesCnt = (int)pagesCnt,
            TotalItems = totalItems
        };
    }

    public async Task<int> GetTotalAnnouncements()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Announcements
            .Where(x => x.ClosedAt != null)
            .CountAsync();
    }
    
    public async Task<int> GetTotalViews()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Views
            .CountAsync();
    }
    
    public async Task<decimal> GetTotalIncome()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Announcements
            .Where(x => x.ClosedAt != null)
            .Select(x => x.StatementNavigation!.Price)
            .SumAsync();
    }
    
    public async Task<GeneralTopDeal?> GetTopDeal()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Announcements
            .Where(x => x.ClosedAt != null)
            .OrderByDescending(x => x.StatementNavigation!.Price)
            .Select(x => new GeneralTopDeal
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
    
    public async Task<List<GeneralTopRealtors>> GetTopRealtors(int top)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Announcements
            .Where(a => a.ClosedAt != null)
            .GroupBy(a => new { a.StatementNavigation!.UserId, a.StatementNavigation.UserNavigation!.Name,  a.StatementNavigation.UserNavigation.Surname})
            .Select(g => new GeneralTopRealtors
            {
                TopRealtorName = g.Key.Name + ' ' + g.Key.Surname,
                TopRealtorDeals = g.Count(),
                TopRealtorIncome = g.Sum(x => x.StatementNavigation!.Price),
            })
            .OrderByDescending(x => x.TopRealtorIncome)
            .Take(top)
            .ToListAsync();
    }
    
    public async Task<List<GeneralTopProperty>> GetTopPropertyTypes(int top)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Announcements
            .Where(a => a.ClosedAt != null)
            .GroupBy(a => new { a.StatementNavigation!.PropertyNavigation!.PropertyTypeId, a.StatementNavigation.PropertyNavigation.PropertyTypeNavigation!.Name})
            .Select(g => new GeneralTopProperty
            {
                TopPropertyTypeName = g.Key.Name,
                TopPropertyTypeCnt = g.Count(),
                TopPropertyTypeAvgPrice = g.Average(x => x.StatementNavigation!.Price),
            })
            .OrderByDescending(x => x.TopPropertyTypeAvgPrice)
            .Take(top)
            .ToListAsync();
    }
    
    public async Task<List<GeneralTopClient>> GetTopClients(int top)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Announcements
            .Where(a => a.ClosedAt != null)
            .GroupBy(a => new { a.PaymentNavigation!.CustomerId, a.PaymentNavigation.CustomerNavigation!.Name, a.PaymentNavigation.CustomerNavigation.Surname})
            .Select(g => new GeneralTopClient
            {
                TopClientName = g.Key.Name +  ' ' + g.Key.Surname,
                TopClientDeals = g.Count(),
                TopClientSpent = g.Sum(x => x.StatementNavigation!.Price),
            })
            .OrderByDescending(x => x.TopClientSpent)
            .Take(top)
            .ToListAsync();
    }
    
    // TODO: imageService
    public async Task<byte[]> GetBytesByAnnouncementIdAsync(Guid announcementId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var announcement = await ctx.Announcements
            .Include(announcement => announcement.StatementNavigation)
            .ThenInclude(statement => statement!.PropertyNavigation)
            .ThenInclude(property => property!.ImagesNavigation)
            .FirstOrDefaultAsync(x => x.Id == announcementId);
        
        // return announcement
        //     ?.StatementNavigation
        //     ?.PropertyNavigation
        //     ?.ImageNavigation
        //     ?.Bytes ?? [];
        return [];
    }
    
    // TODO: imageService
    public async Task<Guid> GetImageIdByAnnouncementIdAsync(Guid announcementId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var announcement = await ctx.Announcements
            .Include(announcement => announcement.StatementNavigation)
            .ThenInclude(statement => statement!.PropertyNavigation)
            .FirstOrDefaultAsync(x => x.Id == announcementId);
        
        // return announcement
        //     ?.StatementNavigation
        //     ?.PropertyNavigation
        //     ?.ImageId ?? Guid.Empty;
        return Guid.Empty;
    }
    
    public async Task<Guid?> GetPropertyIdByAnnouncementIdAsync(Guid announcementId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var announcement = await ctx.Announcements
            .Include(announcement => announcement.StatementNavigation)
            .FirstOrDefaultAsync(x => x.Id == announcementId);
        
        return announcement?.StatementNavigation?.PropertyId;
    }
    
    public async Task<Guid?> GetStatementIdByAnnouncementIdAsync(Guid announcementId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var announcement = await ctx.Announcements
            .Where(x => x.Id == announcementId)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        
        return announcement?.StatementId;
    }
}