using RealEstateAgency.Application.Dto;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IReportsService
{
    Task<GeneralStatsResponseDto> GetGeneralReport();
}