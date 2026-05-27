using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IUserPushSubscriptionRepository
{
    Task<List<UserPushSubscription>> GetAllByUserId(Guid userId);
    Task<UserPushSubscription?> GetByEndpointAsync(string endpoint);
    Task UpdateAsync(UserPushSubscription subscription);
    Task<Guid> Insert(UserPushSubscription model);
    Task Delete(Guid id);
}