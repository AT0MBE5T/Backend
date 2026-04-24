using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class AnalyticRepository(RealEstateContext ctx) : IAnalyticRepository
{
    public async Task<MainStatsDataRawDto> GetMainStats(AnalyticsFilterDto filterDto)
    {
        var query = ctx.Announcements.AsNoTracking();

        query = query.Where(x => x.PublishedAt >= filterDto.DateFrom && x.PublishedAt < filterDto.DateTo);

        if (filterDto.PropertyTypeId != null)
            query = query.Where(x => x.StatementNavigation!.PropertyNavigation!.PropertyTypeId == filterDto.PropertyTypeId);
    
        if (filterDto.StatementTypeId != null)
            query = query.Where(x => x.StatementNavigation!.StatementTypeId == filterDto.StatementTypeId);
        
        var stats = await query.Select(x => new
            {
                IsDeal = x.PaymentNavigation != null,
                IsClosed = x.ClosedAt != null,
                x.StatementNavigation!.Price,
                x.StatementNavigation!.PropertyNavigation!.Area
            })
            .GroupBy(_ => 1)
            .Select(g => new MainStatsDataRawDto
            (
                g.Count(x => x.IsDeal),
                g.Count(x => !x.IsDeal),
                g.Sum(x => x.Price),
                g.Sum(x => x.Area),
                g.Where(x => x.IsDeal).Sum(x => x.Price),
                g.Average(x => x.Price),
                g.Count()
            ))
            .FirstOrDefaultAsync();

        return stats
               ?? new MainStatsDataRawDto(0, 0, 0, 0, 0, 0, 0);
    }
    
    public async Task<List<ChartDataDto>> GetDistributionStatementTypeData(AnalyticsFilterDto filterDto)
    {
        var query = ApplyFilter(ctx.Announcements.AsNoTracking(), filterDto);
        var result = await query
            .GroupBy(a => new { a.StatementNavigation!.StatementTypeId, a.StatementNavigation!.StatementTypeNavigation!.Name })
            .Select(g => new ChartDataDto()
            {
                Label = g.Key.Name,
                Value = g.Count(),
                Id = g.Key.StatementTypeId
            }).ToListAsync();
        return result;
    }
    
    public async Task<List<ChartDataDto>> GetDistributionPropertyTypeData(AnalyticsFilterDto filterDto)
    {
        var query = ApplyFilter(ctx.Announcements.AsNoTracking(), filterDto);
        
        var result = await query
            .GroupBy(a => new { a.StatementNavigation!.PropertyNavigation!.PropertyTypeId, a.StatementNavigation!.PropertyNavigation!.PropertyTypeNavigation!.Name })
            .Select(g => new ChartDataDto()
            {
                Label = g.Key.Name,
                Value = g.Count(),
                Id = g.Key.PropertyTypeId
            }).ToListAsync();
        return result;
    }
    
    public async Task<List<TrendDataDto>> GetMarketTrends(AnalyticsFilterDto filterDto)
    {
        var query = ApplyFilter(ctx.Announcements.AsNoTracking(), filterDto);
        
        var result = await query
            .GroupBy(a => a.PublishedAt.Date) 
            .Select(g => new TrendDataDto
            {
                Date = g.Key,
                AvgPrice = g.Average(y => y.StatementNavigation!.Price),
                Count = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return result;
    }
    
    public async Task<List<AnnouncementGridDto>> GetFilteredAnnouncements(AnalyticsFilterDto filterDto)
    {
        var query = ApplyFilter(ctx.Announcements.AsNoTracking(), filterDto);
        var result = await query
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
            .ToListAsync();
        return result;
    }
    
    public async Task<List<RealtorGridDto>> GetRealtors(AnalyticsFilterDto filterDto)
    {
        var query = ApplyFilter(ctx.Announcements.AsNoTracking(), filterDto);
        var result = await query
            .GroupBy(a => a.StatementNavigation!.UserNavigation!.Id)
            .Select(x => new RealtorGridDto
            {
                Id = x.Key,
                Name = x.Max(b => b.StatementNavigation!.UserNavigation!.Name)!,
                Surname = x.Max(b => b.StatementNavigation!.UserNavigation!.Surname)!,
                Email = x.Max(b => b.StatementNavigation!.UserNavigation!.Email)!,
                Login = x.Max(b => b.StatementNavigation!.UserNavigation!.UserName)!,
                PhoneNumber =  x.Max(b => b.StatementNavigation!.UserNavigation!.PhoneNumber)!,
                Age =  x.Max(b => b.StatementNavigation!.UserNavigation!.Age),
                CreatedAt = x.Max(b => b.StatementNavigation!.UserNavigation!.CreatedAt),
                LockoutEnd =  x.Max(b => b.StatementNavigation!.UserNavigation!.LockoutEnd),
                OffersCnt = x.Count(),
                Revenue = x.Where(b => b.PaymentNavigation != null).Sum(b => b.StatementNavigation!.Price)
            }).ToListAsync();
        return result;
    }
    
    private IQueryable<Announcement> ApplyFilter(IQueryable<Announcement> query, AnalyticsFilterDto filterDto)
    {
        query = query.Where(x => x.PublishedAt >= filterDto.DateFrom && x.PublishedAt <= filterDto.DateTo);
    
        if (filterDto.PropertyTypeId.HasValue)
            query = query.Where(x => x.StatementNavigation!.PropertyNavigation!.PropertyTypeId == filterDto.PropertyTypeId);
        
        if (filterDto.StatementTypeId.HasValue)
            query = query.Where(x => x.StatementNavigation!.StatementTypeId == filterDto.StatementTypeId);
        
        if (filterDto.IsBought)
            query = query.Where(x => x.PaymentNavigation != null);

        return query;
    }
}