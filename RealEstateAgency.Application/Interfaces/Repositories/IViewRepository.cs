using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IViewRepository
{
    Task<bool> IsUserWatchedAsync(Guid userId, Guid announcementId);
    Task<Guid> InsertAsync(View view);
}