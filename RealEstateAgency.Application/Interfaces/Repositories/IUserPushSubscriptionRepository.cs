using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IUserPushSubscriptionRepository
{
    Task<List<UserPushSubscription>> GetAllByUserId(Guid userId);
    Task<Guid> Insert(UserPushSubscription model);
    Task Delete(Guid id);
}