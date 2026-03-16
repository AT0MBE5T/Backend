using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Repositories;

namespace RealEstateAgency.Application.Services;

public class ViewService(IViewRepository viewRepository, ApplicationMapper applicationMapper) : IViewService
{
    public async Task<bool> IsUserWatchedAsync(Guid userId, Guid announcementId)
    {
        var result =  await viewRepository.IsUserWatchedAsync(userId, announcementId);
        return result;
    }

    public async Task<int> GetCntByUserIdAsync(Guid userId)
    {
        var result = await viewRepository.GetCntByUserIdAsync(userId);
        return result;
    }

    public async Task<int> GetCntByOfferIdAsync(Guid announcementId)
    {
        var result = await viewRepository.GetCntByOfferIdAsync(announcementId);
        return result;
    }

    public async Task<Guid> InsertAsync(ViewDto view)
    {
        var isWatched = await IsUserWatchedAsync(view.UserId, view.AnnouncementId);

        if (isWatched)
            return Guid.Empty;
        
        var model = applicationMapper.MapViewDtoToEntity(view);
        var result = await viewRepository.InsertAsync(model);
        return result;
    }

    public async Task<bool> DeleteAsync(ViewDto viewDto)
    {
        var model = await viewRepository.GetDataByUserOfferId(viewDto.AnnouncementId, viewDto.Id);

        if (model is null)
            return false;
        
        var result = await viewRepository.DeleteByIdAsync(model.Id);
        return result;
    }
}