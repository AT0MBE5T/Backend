using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IUserPushSubscriptionService
{
    Task<List<UserPushSubscription>> GetAllByUserIdAsync(Guid userId);
    Task<Guid> AddAsync(UserPushSubscription model);
    Task RemoveByIdAsync(Guid id);
}