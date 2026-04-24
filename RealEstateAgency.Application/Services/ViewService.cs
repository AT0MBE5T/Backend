using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using ApplicationMapper = RealEstateAgency.Application.Mappers.ApplicationMapper;

namespace RealEstateAgency.Application.Services;

public class ViewService(IViewRepository viewRepository, ApplicationMapper applicationMapper) : IViewService
{
    public async Task<bool> IsUserWatchedAsync(Guid userId, Guid announcementId)
    {
        var result =  await viewRepository.IsUserWatchedAsync(userId, announcementId);
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
}