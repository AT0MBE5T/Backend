using RealEstateAgency.Application.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IReportService
{
    Task<GeneralStatsResponseDto> GetGeneralReport();
    Task<PropertyTypeStatsDto?> GetReportByPropertyTypeId(ReportPropertyTypeDto dto);
    Task<PersonalStatsDto?> GetReportByUserLogin(ReportUserDto dto);
    Task<PersonalStatsDto?> GetReportByUserId(Guid userId);
}