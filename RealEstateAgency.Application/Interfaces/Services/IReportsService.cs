using RealEstateAgency.Application.Dto;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IReportsService
{
    Task<GeneralStatsResponseDto> GetGeneralReport();
    Task<PropertyTypeStatsDto?> GetReportByPropertyTypeId(ReportPropertyTypeDto dto);
    Task<PersonalStatsDto?> GetReportByUserLogin(ReportUserDto dto);
    Task<PersonalStatsDto?> GetReportByUserId(Guid userId);
}