using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Services;

public class ReportsService(IAnnouncementRepository repository,  ApplicationMapper mapper) : IReportsService
{
    public async Task<GeneralStatsResponseDto> GetGeneralReport()
    {
        var totalPlacedAnnouncements = await GetTotalAnnouncements();
        var totalIncome = await GetTotalIncome();
        var topDeal = await GetTopDeal() ?? new GeneralTopDeal();
        var topRealtors = await GetTopRealtors();
        var topPropertyTypes = await GetTopPropertyTypes();
        var topClients = await GetTopClients();

        var res = mapper.ToGeneralStatsResponseDto(topDeal, topRealtors, topPropertyTypes, topClients, totalPlacedAnnouncements, totalIncome);

        return res;
    }

    private async Task<int> GetTotalAnnouncements()
    {
        return await repository.GetTotalAnnouncements();
    }

    private async Task<decimal> GetTotalIncome()
    {
        return await repository.GetTotalIncome();
    }
    
    private async Task<GeneralTopDeal?> GetTopDeal()
    {
        return await repository.GetTopDeal();
    }
    
    private async Task<List<GeneralTopRealtors>> GetTopRealtors()
    {
        return await repository.GetTopRealtors(2);
    }

    private async Task<List<GeneralTopProperty>> GetTopPropertyTypes()
    {
        return await repository.GetTopPropertyTypes(2);
    }
    
    private async Task<List<GeneralTopClient>> GetTopClients()
    {
        return await repository.GetTopClients(2);
    }
}