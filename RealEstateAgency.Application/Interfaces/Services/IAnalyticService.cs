using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Services;

public interface IAnalyticService
{
    Task<MainStatsDto> GetMainStats(AnalyticsFilterDto filterDto);
    Task<List<ChartDataDto>> GetDistributionStatementTypeData(AnalyticsFilterDto filterDto);
    Task<List<ChartDataDto>> GetDistributionPropertyTypeData(AnalyticsFilterDto filterDto);
    Task<List<TrendDataDto>> GetMarketTrends(AnalyticsFilterDto filterDto);
    Task<List<AnnouncementGridDto>> GetFilteredAnnouncements(AnalyticsFilterDto filterDto);
    Task<List<RealtorGridDto>> GetRealtors(AnalyticsFilterDto filterDto);
}