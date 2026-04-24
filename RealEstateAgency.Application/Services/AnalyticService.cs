using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Services;

public class AnalyticService(IAnalyticRepository repository) : IAnalyticService
{
    public async Task<MainStatsDto> GetMainStats(AnalyticsFilterDto filterDto)
    {
        var raw = await repository.GetMainStats(filterDto);

        var result = new MainStatsDto
        {
            TotalDeals = raw.TotalDeals,
            TotalActive = raw.TotalActive,
            Revenue = raw.Revenue,
            AvgPricePerMeter = raw.SumArea > 0 
                ? raw.SumPrice / (decimal)raw.SumArea 
                : 0
        };

        return result;
    }
    
    public async Task<List<ChartDataDto>> GetDistributionStatementTypeData(AnalyticsFilterDto filterDto)
    {
        filterDto.DateFrom = DateTime.SpecifyKind(filterDto.DateFrom, DateTimeKind.Utc);
        filterDto.DateTo = DateTime.SpecifyKind(filterDto.DateTo, DateTimeKind.Utc);
        var result = await repository.GetDistributionStatementTypeData(filterDto);
        return result;
    }
    
    public async Task<List<ChartDataDto>> GetDistributionPropertyTypeData(AnalyticsFilterDto filterDto)
    {
        filterDto.DateFrom = DateTime.SpecifyKind(filterDto.DateFrom, DateTimeKind.Utc);
        filterDto.DateTo = DateTime.SpecifyKind(filterDto.DateTo, DateTimeKind.Utc);
        var result = await repository.GetDistributionPropertyTypeData(filterDto);
        return result;
    }
    
    public async Task<List<TrendDataDto>> GetMarketTrends(AnalyticsFilterDto filterDto)
    {
        filterDto.DateFrom = DateTime.SpecifyKind(filterDto.DateFrom, DateTimeKind.Utc);
        filterDto.DateTo = DateTime.SpecifyKind(filterDto.DateTo, DateTimeKind.Utc);
        var result = await repository.GetMarketTrends(filterDto);
        return result;
    }
    
    public async Task<List<AnnouncementGridDto>> GetFilteredAnnouncements(AnalyticsFilterDto filterDto)
    {
        filterDto.DateFrom = DateTime.SpecifyKind(filterDto.DateFrom, DateTimeKind.Utc);
        filterDto.DateTo = DateTime.SpecifyKind(filterDto.DateTo, DateTimeKind.Utc);
        var result = await repository.GetFilteredAnnouncements(filterDto);
        return result;
    }
    
    public async Task<List<RealtorGridDto>> GetRealtors(AnalyticsFilterDto filterDto)
    {
        filterDto.DateFrom = DateTime.SpecifyKind(filterDto.DateFrom, DateTimeKind.Utc);
        filterDto.DateTo = DateTime.SpecifyKind(filterDto.DateTo, DateTimeKind.Utc);
        var result = await repository.GetRealtors(filterDto);
        return result;
    }
}