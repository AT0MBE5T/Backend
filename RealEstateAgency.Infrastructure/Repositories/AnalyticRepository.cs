using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class AnalyticRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IAnalyticRepository
{
    public async Task<MainStatsDto> GetMainStats(AnalyticsFilter filter)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();

        var query = ctx.Announcements.AsNoTracking();

        query = query.Where(x => x.PublishedAt >= filter.DateFrom && x.PublishedAt < filter.DateTo);

        if (filter.PropertyTypeId != null)
            query = query.Where(x => x.StatementNavigation.PropertyNavigation.PropertyTypeId == filter.PropertyTypeId);
    
        if (filter.StatementTypeId != null)
            query = query.Where(x => x.StatementNavigation.StatementTypeId == filter.StatementTypeId);
        
        var stats = await query.Select(x => new
            {
                IsDeal = x.PaymentNavigation != null,
                IsClosed = x.ClosedAt != null,
                Price = x.StatementNavigation.Price,
                Area = x.StatementNavigation.PropertyNavigation.Area
            })
            .GroupBy(_ => 1)
            .Select(g => new
            {
                TotalDeals = g.Count(x => x.IsDeal),
                TotalActive = g.Count(x => !x.IsDeal),
                SumPrice = g.Sum(x => x.Price),
                SumArea = g.Sum(x => x.Area),
                Revenue = g.Where(x => x.IsDeal).Sum(x => x.Price),
                AvgPrice = g.Average(x => x.Price),
                Count = g.Count()
            })
            .FirstOrDefaultAsync();

        if (stats == null) return new MainStatsDto();

        return new MainStatsDto
        {
            TotalDeals = stats.TotalDeals,
            TotalActive = stats.TotalActive,
            Revenue = stats.Revenue,
            AvgPricePerMeter = stats.SumArea > 0 ? stats.SumPrice / (decimal)stats.SumArea : 0
        };
    }
    
    public async Task<List<ChartDataDto>> GetDistributionStatementTypeData(AnalyticsFilter filter)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var query = ApplyFilter(ctx.Announcements.AsNoTracking(), filter);
        var result = await query
            .GroupBy(a => new { a.StatementNavigation!.StatementTypeId, a.StatementNavigation!.StatementTypeNavigation.Name })
            .Select(g => new ChartDataDto()
            {
                Label = g.Key.Name,
                Value = g.Count(),
                Id = g.Key.StatementTypeId
            }).ToListAsync();
        return result;
    }
    
    public async Task<List<ChartDataDto>> GetDistributionPropertyTypeData(AnalyticsFilter filter)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var query = ApplyFilter(ctx.Announcements.AsNoTracking(), filter);
        
        var result = await query
            .GroupBy(a => new { a.StatementNavigation!.PropertyNavigation.PropertyTypeId, a.StatementNavigation!.PropertyNavigation.PropertyTypeNavigation.Name })
            .Select(g => new ChartDataDto()
            {
                Label = g.Key.Name,
                Value = g.Count(),
                Id = g.Key.PropertyTypeId
            }).ToListAsync();
        return result;
    }
    
    public async Task<List<TrendDataDto>> GetMarketTrends(AnalyticsFilter filter)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
    
        var query = ApplyFilter(ctx.Announcements.AsNoTracking(), filter);
        
        var result = await query
            .GroupBy(a => a.PublishedAt.Date) 
            .Select(g => new TrendDataDto
            {
                Date = g.Key,
                AvgPrice = g.Average(y => y.StatementNavigation.Price),
                Count = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return result;
    }
    
    public async Task<List<AnnouncementGrid>> GetFilteredAnnouncements(AnalyticsFilter filter)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var query = ApplyFilter(ctx.Announcements.AsNoTracking(), filter);
        var result = await query
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
    
    public async Task<List<RealtorGrid>> GetRealtors(AnalyticsFilter filter)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var query = ApplyFilter(ctx.Announcements.AsNoTracking(), filter);
        var result = await query
            .GroupBy(a => a.StatementNavigation.UserNavigation.Id)
            .Select(x => new RealtorGrid
            {
                Id = x.Key,
                Name = x.Max(b => b.StatementNavigation.UserNavigation.Name),
                Surname = x.Max(b => b.StatementNavigation.UserNavigation.Surname),
                Email = x.Max(b => b.StatementNavigation.UserNavigation.Email),
                Login = x.Max(b => b.StatementNavigation.UserNavigation.UserName),
                PhoneNumber =  x.Max(b => b.StatementNavigation.UserNavigation.PhoneNumber),
                Age =  x.Max(b => b.StatementNavigation.UserNavigation.Age),
                CreatedAt = x.Max(b => b.StatementNavigation.UserNavigation.CreatedAt),
                LockoutEnd =  x.Max(b => b.StatementNavigation.UserNavigation.LockoutEnd),
                OffersCnt = x.Count(),
                Revenue = x.Where(b => b.PaymentNavigation != null).Sum(b => b.StatementNavigation.Price)
            }).ToListAsync();
        return result;
    }
    
    private IQueryable<Announcement> ApplyFilter(IQueryable<Announcement> query, AnalyticsFilter filter)
    {
        query = query.Where(x => x.PublishedAt >= filter.DateFrom && x.PublishedAt <= filter.DateTo);
    
        if (filter.PropertyTypeId.HasValue)
            query = query.Where(x => x.StatementNavigation.PropertyNavigation.PropertyTypeId == filter.PropertyTypeId);
        
        if (filter.StatementTypeId.HasValue)
            query = query.Where(x => x.StatementNavigation.StatementTypeId == filter.StatementTypeId);
        
        if (filter.IsBought)
            query = query.Where(x => x.PaymentNavigation != null);

        return query;
    }
}