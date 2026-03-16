using System.Globalization;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;

namespace RealEstateAgency.Application.Services;

public class PropertyTypeService(IPropertyTypeRepository repository, ApplicationMapper mapper) : IPropertyTypeService
{
    public async Task<List<PropertyTypeDto>> GetAll()
    {
        var statements = await repository.GetAllAsync();
        return statements
            .Select(mapper.PropertyTypeEntityToPropertyTypeDto)
            .ToList();
    }

    private async Task<PropertyTypeTotalsDto> GetTotalsDate(Guid propertyTypeId, DateTime date)
    {
        var totalPlacedAnnouncements = await repository.GetTotalPlacedAnnouncementsDate(propertyTypeId, date);
        var totalDeals = await repository.GetTotalDealsDate(propertyTypeId, date);
        var totalIncome = await repository.GetTotalIncomeDate(propertyTypeId, date);

        return new PropertyTypeTotalsDto
        {
            TotalPlacedAnnouncements = totalPlacedAnnouncements,
            TotalDeals = totalDeals,
            TotalIncome = totalIncome
        };
    }
    
    private async Task<PropertyTypeTotalsDto> GetTotalsDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo)
    {
        var totalPlacedAnnouncements = await repository.GetTotalPlacedAnnouncementsDateSpan(propertyTypeId, dateFrom, dateTo);
        var totalDeals = await repository.GetTotalDealsDateSpan(propertyTypeId, dateFrom, dateTo);
        var totalIncome = await repository.GetTotalIncomeDateSpan(propertyTypeId, dateFrom, dateTo);

        return new PropertyTypeTotalsDto
        {
            TotalPlacedAnnouncements = totalPlacedAnnouncements,
            TotalDeals = totalDeals,
            TotalIncome = totalIncome
        };
    }
    
    private async Task<PropertyTypeTopDealDto> GetTopDealDate(Guid propertyTypeId, DateTime date)
    {
        var data = await repository.GetTopDealDate(propertyTypeId, date);
        
        return new PropertyTypeTopDealDto
        {
            TopDealName = data?.TopDealName ?? string.Empty,
            TopDealStatementType = data?.TopDealStatementType ??  string.Empty,
            TopDealCustomerName = data?.TopDealCustomerName ?? string.Empty,
            TopDealRealtorName = data?.TopDealRealtorName ?? string.Empty,
            TopDealSoldDate = data?.TopDealSoldDate.ToString(CultureInfo.InvariantCulture) ??  string.Empty,
            TopDealPrice = data?.TopDealPrice ?? 0
        };
    }
    
    private async Task<PropertyTypeTopDealDto> GetTopDealDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo)
    {
        var data = await repository.GetTopDealDateSpan(propertyTypeId, dateFrom, dateTo);
        return new PropertyTypeTopDealDto
        {
            TopDealName = data?.TopDealName ?? string.Empty,
            TopDealStatementType = data?.TopDealStatementType ?? string.Empty,
            TopDealCustomerName = data?.TopDealCustomerName ?? string.Empty,
            TopDealRealtorName = data?.TopDealRealtorName ?? string.Empty,
            TopDealSoldDate = data?.TopDealSoldDate.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
            TopDealPrice = data?.TopDealPrice ?? 0
        };
    }
    
    private async Task<PropertyTypeTopRealtorDto> GetTopRealtorDate(Guid propertyTypeId, DateTime date)
    {
        var data = await repository.GetTopRealtorDate(propertyTypeId, date);
        return new PropertyTypeTopRealtorDto
        {
            TopRealtorName = data?.TopRealtorName ?? string.Empty,
            TopRealtorDeals = data?.TopRealtorDeals ?? 0,
            TopRealtorIncome = data?.TopRealtorIncome ?? 0
        };
    }
    
    private async Task<PropertyTypeTopRealtorDto> GetTopRealtorDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo)
    {
        var data = await repository.GetTopRealtorDateSpan(propertyTypeId, dateFrom, dateTo);
        return new PropertyTypeTopRealtorDto
        {
            TopRealtorName = data?.TopRealtorName ?? string.Empty,
            TopRealtorDeals = data?.TopRealtorDeals ?? 0,
            TopRealtorIncome = data?.TopRealtorIncome ?? 0
        };
    }
    
    private async Task<PropertyTypeTopClientDto> GetTopClientDate(Guid propertyTypeId, DateTime date)
    {
        var data = await repository.GetTopClientDate(propertyTypeId, date);
        return new PropertyTypeTopClientDto
        {
            TopClientDeals = data?.TopClientDeals ?? 0,
            TopClientName = data?.TopClientName ?? string.Empty,
            TopClientSpent = data?.TopClientSpent ?? 0
        };
    }
    
    private async Task<PropertyTypeTopClientDto> GetTopClientDateSpan(Guid propertyTypeId, DateTime dateFrom , DateTime dateTo)
    {
        var data = await repository.GetTopClientDateSpan(propertyTypeId, dateFrom, dateTo);
        return new PropertyTypeTopClientDto
        {
            TopClientDeals = data?.TopClientDeals ?? 0,
            TopClientName = data?.TopClientName ?? string.Empty,
            TopClientSpent = data?.TopClientSpent ?? 0
        };
    }
    
    public async Task<PropertyTypeStatsDto?> GetReportByPropertyTypeDate(ReportPropertyTypeDto reportPropertyTypeDto)
    {
        var totals = await GetTotalsDate(reportPropertyTypeDto.PropertyTypeId, reportPropertyTypeDto.DateFrom);
        var topDeal = await GetTopDealDate(reportPropertyTypeDto.PropertyTypeId, reportPropertyTypeDto.DateFrom);
        var topRealtor = await GetTopRealtorDate(reportPropertyTypeDto.PropertyTypeId, reportPropertyTypeDto.DateFrom);
        var topClient = await GetTopClientDate(reportPropertyTypeDto.PropertyTypeId, reportPropertyTypeDto.DateFrom);

        var result = mapper.ToPropertyTypeStatsDto(totals, topDeal, topRealtor, topClient);

        return result;
    }
    
    public async Task<PropertyTypeStatsDto?> GetReportByPropertyTypeDateSpan(ReportPropertyTypeDto reportPropertyTypeDto)
    {
        var totals = await GetTotalsDateSpan(reportPropertyTypeDto.PropertyTypeId, reportPropertyTypeDto.DateFrom, reportPropertyTypeDto.DateTo);
        var topDeal = await GetTopDealDateSpan(reportPropertyTypeDto.PropertyTypeId, reportPropertyTypeDto.DateFrom, reportPropertyTypeDto.DateTo);
        var topRealtor = await GetTopRealtorDateSpan(reportPropertyTypeDto.PropertyTypeId, reportPropertyTypeDto.DateFrom, reportPropertyTypeDto.DateTo);
        var topClient = await GetTopClientDateSpan(reportPropertyTypeDto.PropertyTypeId, reportPropertyTypeDto.DateFrom, reportPropertyTypeDto.DateTo);
        
        var result = mapper.ToPropertyTypeStatsDto(totals, topDeal, topRealtor, topClient);

        return result;
    }
}