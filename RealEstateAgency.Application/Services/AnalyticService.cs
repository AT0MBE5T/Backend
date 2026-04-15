using RealEstateAgency.Core.DTO;
using RealEstateAgency.Infrastructure.Repositories;

namespace RealEstateAgency.Application.Services;

public class AnalyticService(IAnalyticRepository repository) : IAnalyticService
{
    public async Task<MainStatsDto> GetMainStats(AnalyticsFilter filter)
    {
        var result = await repository.GetMainStats(filter);
        return result;
    }
    
    public async Task<List<ChartDataDto>> GetDistributionStatementTypeData(AnalyticsFilter filter)
    {
        filter.DateFrom = DateTime.SpecifyKind(filter.DateFrom, DateTimeKind.Utc);
        filter.DateTo = DateTime.SpecifyKind(filter.DateTo, DateTimeKind.Utc);
        var result = await repository.GetDistributionStatementTypeData(filter);
        return result;
    }
    
    public async Task<List<ChartDataDto>> GetDistributionPropertyTypeData(AnalyticsFilter filter)
    {
        filter.DateFrom = DateTime.SpecifyKind(filter.DateFrom, DateTimeKind.Utc);
        filter.DateTo = DateTime.SpecifyKind(filter.DateTo, DateTimeKind.Utc);
        var result = await repository.GetDistributionPropertyTypeData(filter);
        return result;
    }
    
    public async Task<List<TrendDataDto>> GetMarketTrends(AnalyticsFilter filter)
    {
        filter.DateFrom = DateTime.SpecifyKind(filter.DateFrom, DateTimeKind.Utc);
        filter.DateTo = DateTime.SpecifyKind(filter.DateTo, DateTimeKind.Utc);
        var result = await repository.GetMarketTrends(filter);
        return result;
    }
    
    public async Task<List<AnnouncementGrid>> GetFilteredAnnouncements(AnalyticsFilter filter)
    {
        filter.DateFrom = DateTime.SpecifyKind(filter.DateFrom, DateTimeKind.Utc);
        filter.DateTo = DateTime.SpecifyKind(filter.DateTo, DateTimeKind.Utc);
        var result = await repository.GetFilteredAnnouncements(filter);
        return result;
    }
    
    public async Task<List<RealtorGrid>> GetRealtors(AnalyticsFilter filter)
    {
        filter.DateFrom = DateTime.SpecifyKind(filter.DateFrom, DateTimeKind.Utc);
        filter.DateTo = DateTime.SpecifyKind(filter.DateTo, DateTimeKind.Utc);
        var result = await repository.GetRealtors(filter);
        return result;
    }
}