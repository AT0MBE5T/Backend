using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IUserPushSubscriptionService
{
    Task<List<UserPushSubscription>> GetAllByUserIdAsync(Guid userId);
    Task<Guid> AddAsync(UserPushSubscription model);
    Task UpdateAsync(UserPushSubscription newData);
    Task<UserPushSubscription?> GetByEndpointAsync(string endpoint);
    Task RemoveByIdAsync(Guid id);
}