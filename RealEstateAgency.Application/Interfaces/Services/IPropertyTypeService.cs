using RealEstateAgency.Application.Dto;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IPropertyTypeService
{
    Task<List<PropertyTypeDto>> GetAll();
    Task<PropertyTypeStatsDto?> GetReportByPropertyTypeDate(ReportPropertyTypeDto reportPropertyTypeDto);
    Task<PropertyTypeStatsDto?> GetReportByPropertyTypeDateSpan(ReportPropertyTypeDto reportPropertyTypeDto);
}