using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class FavoriteRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IFavoriteRepository
{
    public async Task<bool> IsFavorited(Guid userId, Guid offerId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var result = await ctx.Favorites
            .AnyAsync(x => x.UserId == userId && x.AnnouncementId == offerId);

        return result;
    }
    
    public async Task<AnnouncementsShortAndPages> GetFavoritesByUserId(Guid userId, int pageNumber, int pageSize)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var query = ctx.Favorites
            .Join(ctx.Announcements, f => f.AnnouncementId, a => a.Id, (f,a) => new {f, a})
            .Where(x => x.f.UserId == userId);
        
        var pagesCnt = Math.Ceiling((double)query.Count() / pageSize);
        var totalItems = await query.CountAsync();
        
        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AnnouncementShort
            {
                Id = x.a.Id,
                Title = x.a.StatementNavigation!.Title,
                StatementTypeName = x.a.StatementNavigation.StatementTypeNavigation!.Name,
                Price = x.a.StatementNavigation.Price,
                Area = x.a.StatementNavigation.PropertyNavigation!.Area,
                Location = x.a.StatementNavigation.PropertyNavigation.Location,
                PhotoUrl = x.a.StatementNavigation.PropertyNavigation.ImagesNavigation!.FirstOrDefault().PhotoUrl,
                PropertyTypeName = x.a.StatementNavigation.PropertyNavigation.PropertyTypeNavigation!.Name,
                IsVerified = x.a.VerificationNavigation != null,
                IsFavorite = ctx.Favorites
                    .Any(f => f.AnnouncementId == x.a.Id && f.UserId == userId),
                ViewsCnt = ctx.Views.Count(v => v.AnnouncementId == x.a.Id),
                ClosedAt = x.a.ClosedAt
            }).ToListAsync();
        return new AnnouncementsShortAndPages
        {
            Data = data,
            PagesCnt = (int)pagesCnt,
            TotalItems = totalItems
        };
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
            5 => query.OrderBy(x => x.StatementNavigation!.Price),
            6 => query.OrderBy(x => x.StatementNavigation!.PropertyNavigation!.Area),
            7 => query.OrderBy(x => x.StatementNavigation!.PropertyNavigation!.Rooms),
            8 => query.OrderBy(x => x.StatementNavigation!.PropertyNavigation!.Floors),
            _ => query
        };
    }
    
    public async Task<AnnouncementsShortAndPages> GetSearchDataAsync(Guid userId, string text, List<string> filtersId, int sortId, int pageNumber, int pageSize)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var query = ctx.Favorites
            .Join(ctx.Announcements, f => f.AnnouncementId, a => a.Id, (f, a) => a)
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
                PhotoUrl = x.StatementNavigation.PropertyNavigation.ImagesNavigation.FirstOrDefault()!.PhotoUrl,
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
    
    public async Task<bool> AddAsync(Favorite entity)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            var res = await ctx.Favorites.AddAsync(entity);
            await ctx.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<bool> DeleteByIdAsync(Guid userId, Guid offerId)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            var res = await ctx.Favorites
                .Where(x => x.UserId == userId && x.AnnouncementId == offerId)
                .ExecuteDeleteAsync();
            return res != 0;
        }
        catch
        {
            return false;
        }
    }
}