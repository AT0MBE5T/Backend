using RealEstateAgency.Application.Dto;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IViewService
{
    Task<bool> IsUserWatchedAsync(Guid userId, Guid announcementId);
    Task<int> GetCntByUserIdAsync(Guid userId);
    Task<int> GetCntByOfferIdAsync(Guid announcementId);
    Task<Guid> InsertAsync(ViewDto view);
    Task<bool> DeleteAsync(ViewDto viewDto);
}