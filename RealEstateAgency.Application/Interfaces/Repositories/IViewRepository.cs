using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Repositories;

public interface IViewRepository
{
    Task<bool> IsUserWatchedAsync(Guid userId, Guid announcementId);
    Task<int> GetCntByUserIdAsync(Guid userId);
    Task<int> GetCntByOfferIdAsync(Guid announcementId);
    Task<View?> GetDataByUserOfferId(Guid userId, Guid offerId);
    Task<Guid> InsertAsync(View view);
    Task<bool> DeleteByIdAsync(Guid id);
}