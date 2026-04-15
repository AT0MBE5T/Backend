using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Services;

public interface IAnalyticService
{
    Task<MainStatsDto> GetMainStats(AnalyticsFilter filter);
    Task<List<ChartDataDto>> GetDistributionStatementTypeData(AnalyticsFilter filter);
    Task<List<ChartDataDto>> GetDistributionPropertyTypeData(AnalyticsFilter filter);
    Task<List<TrendDataDto>> GetMarketTrends(AnalyticsFilter filter);
    Task<List<AnnouncementGrid>> GetFilteredAnnouncements(AnalyticsFilter filter);
    Task<List<RealtorGrid>> GetRealtors(AnalyticsFilter filter);
}