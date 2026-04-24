using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IAnalyticRepository
{
    Task<MainStatsDataRawDto> GetMainStats(AnalyticsFilterDto filterDto);
    Task<List<ChartDataDto>> GetDistributionStatementTypeData(AnalyticsFilterDto filterDto);
    Task<List<ChartDataDto>> GetDistributionPropertyTypeData(AnalyticsFilterDto filterDto);
    Task<List<TrendDataDto>> GetMarketTrends(AnalyticsFilterDto filterDto);
    Task<List<AnnouncementGridDto>> GetFilteredAnnouncements(AnalyticsFilterDto filterDto);
    Task<List<RealtorGridDto>> GetRealtors(AnalyticsFilterDto filterDto);
}