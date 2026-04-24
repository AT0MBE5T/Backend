using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class PropertyTypeRepository(RealEstateContext ctx) : IPropertyTypeRepository
{
    public async Task<List<PropertyType>> GetAllAsync()
    {
        var result = await ctx.PropertyTypes.AsNoTracking().ToListAsync();
        return result;
    }

    public async Task<int> GetTotalPlacedAnnouncementsDate(Guid propertyTypeId, DateTime date)
    {
        var start = date.ToUniversalTime().AddDays(1).Date;
        var end = start.AddDays(1);

        var result = await ctx.Announcements
            .Where(x => x.StatementNavigation!.PropertyNavigation!.PropertyTypeId == propertyTypeId && x.PublishedAt.Date >= start && x.PublishedAt.Date < end)
            .CountAsync();
        return result;
    }

    public async Task<int> GetTotalDealsDate(Guid propertyTypeId, DateTime date)
    {
        var start = date.ToUniversalTime().AddDays(1).Date;
        var end = start.AddDays(1);
        
        var result = await ctx.Announcements
            .Where(x =>
                x.StatementNavigation!.PropertyNavigation!.PropertyTypeId == propertyTypeId
                && x.ClosedAt!.Value.Date >= start
                && x.ClosedAt.Value.Date < end)
            .CountAsync();
        return result;
    }

    public async Task<decimal> GetTotalIncomeDate(Guid propertyTypeId, DateTime date)
    {
        var start = date.ToUniversalTime().AddDays(1).Date;
        var end = start.AddDays(1);
        
        var result = await ctx.Payments
            .Where(x =>
                x.AnnouncementNavigation!.StatementNavigation!.PropertyNavigation!.PropertyTypeId == propertyTypeId
                && x.AnnouncementNavigation.ClosedAt!.Value.Date >= start
                && x.AnnouncementNavigation.ClosedAt.Value.Date < end)
            .Select(x => x.AnnouncementNavigation!.StatementNavigation!.Price)
            .SumAsync();
        return result;
    }
    
    public async Task<int> GetViewsByPropertyIdDate(Guid propertyTypeId, DateTime date)
    {
        var start = date.ToUniversalTime().AddDays(1).Date;
        var end = start.AddDays(1);
        
        var result = await ctx.Views
            .Where(x => x.CreatedAt >= start
                        && x.CreatedAt < end
                        && x.AnnouncementNavigation!
                            .StatementNavigation
                            !.PropertyNavigation
                            !.PropertyTypeId == propertyTypeId)
            .CountAsync();

        return result;
    }
    
    public async Task<int> GetViewsByPropertyIdDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo)
    {
        var start = dateFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTo.ToUniversalTime().Date.AddDays(2);
        
        var result = await ctx.Views
            .Where(x => x.CreatedAt >= start
                        && x.CreatedAt < end
                        && x.AnnouncementNavigation!
                            .StatementNavigation
                            !.PropertyNavigation
                            !.PropertyTypeId == propertyTypeId)
            .CountAsync();

        return result;
    }

    public async Task<int> GetTotalPlacedAnnouncementsDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo)
    {
        var start = dateFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTo.ToUniversalTime().Date.AddDays(2);
        
        var result = await ctx.Announcements
            .Where(x =>
                x.StatementNavigation!.PropertyNavigation!.PropertyTypeId == propertyTypeId
                && x.PublishedAt.Date >= start
                && x.PublishedAt.Date < end)
            .CountAsync();
        return result;
    }

    public async Task<int> GetTotalDealsDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo)
    {
        var start = dateFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTo.ToUniversalTime().Date.AddDays(2);
        
        var result = await ctx.Announcements
            .Where(x =>
                x.StatementNavigation!.PropertyNavigation!.PropertyTypeId == propertyTypeId
                && x.ClosedAt!.Value.Date >= start
                && x.ClosedAt.Value.Date < end)
            .CountAsync();
        return result;
    }

    public async Task<decimal> GetTotalIncomeDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo)
    {
        var start = dateFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTo.ToUniversalTime().Date.AddDays(2);
        
        var result = await ctx.Payments
            .Where(x =>
                x.AnnouncementNavigation!.StatementNavigation!.PropertyNavigation!.PropertyTypeId == propertyTypeId
                && x.AnnouncementNavigation.ClosedAt!.Value.Date >= start
                && x.AnnouncementNavigation.ClosedAt.Value.Date < end)
            .Select(x => x.AnnouncementNavigation!.StatementNavigation!.Price)
            .SumAsync();

        return result;
    }

    public async Task<PropertyTypeTopDealCoreDto?> GetTopDealDate(Guid propertyTypeId, DateTime date)
    {
        var start = date.ToUniversalTime().AddDays(1).Date;
        var end = start.AddDays(1);

        var result = await ctx.Announcements
            .Where(x => x.StatementNavigation!.PropertyNavigation!.PropertyTypeId == propertyTypeId &&
                        x.ClosedAt!.Value.Date >= start && x.ClosedAt.Value.Date < end)
            .OrderByDescending(x => x.StatementNavigation!.Price)
            .Select(x => new PropertyTypeTopDealCoreDto
            {
                TopDealName = x.StatementNavigation!.Title,
                TopDealStatementType = x.StatementNavigation.StatementTypeNavigation!.Name,
                TopDealCustomerName = x.PaymentNavigation!.CustomerNavigation!.Name,
                TopDealSoldDate = x.ClosedAt!.Value.Date,
                TopDealRealtorName = x.StatementNavigation.UserNavigation!.Name + ' ' + x.StatementNavigation.UserNavigation.Surname,
                TopDealPrice = x.StatementNavigation.Price,
            })
            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<PropertyTypeTopDealCoreDto?> GetTopDealDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo)
    {
        var start = dateFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTo.ToUniversalTime().Date.AddDays(2);
        
        var result = await ctx.Announcements
            .Where(x => x.StatementNavigation!.PropertyNavigation!.PropertyTypeId == propertyTypeId &&
                        x.ClosedAt!.Value.Date >= start && x.ClosedAt.Value.Date < end)
            .OrderByDescending(x => x.StatementNavigation!.Price)
            .Select(x => new PropertyTypeTopDealCoreDto
            {
                TopDealName = x.StatementNavigation!.Title,
                TopDealStatementType = x.StatementNavigation.StatementTypeNavigation!.Name,
                TopDealCustomerName = x.PaymentNavigation!.CustomerNavigation!.Name + ' ' + x.PaymentNavigation.CustomerNavigation.Surname,
                TopDealSoldDate = x.ClosedAt!.Value.Date,
                TopDealRealtorName = x.StatementNavigation.UserNavigation!.Name + ' ' + x.StatementNavigation.UserNavigation.Surname,
                TopDealPrice = x.StatementNavigation.Price,
            })
            .FirstOrDefaultAsync();
        return result;
    }
    
    public async Task<PropertyTypeTopRealtorCoreDto?> GetTopRealtorDate(Guid propertyTypeId, DateTime date)
    {
        var start = date.ToUniversalTime().AddDays(1).Date;
        var end = start.AddDays(1);
        
        var result = await ctx.Announcements
            .Where(a => a.StatementNavigation!.PropertyNavigation!.PropertyTypeId == propertyTypeId && a.ClosedAt >= start && a.ClosedAt < end)
            .GroupBy(a => new { a.StatementNavigation!.UserId, a.StatementNavigation.UserNavigation!.Name,  a.StatementNavigation.UserNavigation.Surname})
            .Select(g => new PropertyTypeTopRealtorCoreDto
            {
                TopRealtorName = g.Key.Name + ' ' + g.Key.Surname,
                TopRealtorDeals = g.Count(),
                TopRealtorIncome = g.Sum(x => x.StatementNavigation!.Price),
            })
            .OrderByDescending(x => x.TopRealtorIncome)
            .FirstOrDefaultAsync();
        return result;
    }
    
    public async Task<PropertyTypeTopRealtorCoreDto?> GetTopRealtorDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo)
    {
        var start = dateFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTo.ToUniversalTime().Date.AddDays(2);
        
        var result = await ctx.Announcements
            .Where(a => a.StatementNavigation!.PropertyNavigation!.PropertyTypeId == propertyTypeId && a.ClosedAt >= start && a.ClosedAt < end)
            .GroupBy(a => new { a.StatementNavigation!.UserId, a.StatementNavigation!.UserNavigation!.Name,  a.StatementNavigation.UserNavigation.Surname})
            .Select(g => new PropertyTypeTopRealtorCoreDto
            {
                TopRealtorName = g.Key.Name + ' ' + g.Key.Surname,
                TopRealtorDeals = g.Count(),
                TopRealtorIncome = g.Sum(x => x.StatementNavigation!.Price),
            })
            .OrderByDescending(x => x.TopRealtorIncome)
            .FirstOrDefaultAsync();
        return result;
    }
    
    public async Task<PropertyTypeTopClientCoreDto?> GetTopClientDate(Guid propertyTypeId, DateTime date)
    {
        var start = date.ToUniversalTime().AddDays(1).Date;
        var end = start.AddDays(1);
        
        var result = await ctx.Announcements
            .Where(a => a.StatementNavigation!.PropertyNavigation!.PropertyTypeId == propertyTypeId && a.ClosedAt >= start && a.ClosedAt < end)
            .GroupBy(a => new { a.PaymentNavigation!.CustomerId, a.PaymentNavigation.CustomerNavigation!.Name, a.PaymentNavigation.CustomerNavigation.Surname})
            .Select(g => new PropertyTypeTopClientCoreDto
            {
                TopClientName = g.Key.Name + ' ' + g.Key.Surname,
                TopClientDeals = g.Count(),
                TopClientSpent = g.Sum(x => x.StatementNavigation!.Price),
            })
            .OrderByDescending(x => x.TopClientSpent)
            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<PropertyTypeTopClientCoreDto?> GetTopClientDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo)
    {
        var start = dateFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTo.ToUniversalTime().Date.AddDays(2);

        var result = await ctx.Announcements
            .Where(a => a.StatementNavigation!.PropertyNavigation!.PropertyTypeId == propertyTypeId && a.ClosedAt >= start && a.ClosedAt < end)
            .GroupBy(a => new { a.PaymentNavigation!.CustomerId, a.PaymentNavigation.CustomerNavigation!.Name, a.PaymentNavigation.CustomerNavigation.Surname})
            .Select(g => new PropertyTypeTopClientCoreDto
            {
                TopClientName = g.Key.Name + ' ' + g.Key.Surname,
                TopClientDeals = g.Count(),
                TopClientSpent = g.Sum(x => x.StatementNavigation!.Price),
            })
            .OrderByDescending(x => x.TopClientSpent)
            .FirstOrDefaultAsync();
        return result;
    }
}