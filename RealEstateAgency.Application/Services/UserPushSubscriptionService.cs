using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Services;

public class UserPushSubscriptionService(IUserPushSubscriptionRepository repository) : IUserPushSubscriptionService
{
    public async Task<List<UserPushSubscription>> GetAllByUserIdAsync(Guid userId)
    {
        var result = await repository.GetAllByUserId(userId);
        return result;
    }
    
    public async Task<Guid> AddAsync(UserPushSubscription model)
    {
        var result = await repository.Insert(model);
        return result;
    }

    public async Task RemoveByIdAsync(Guid id)
    {
        await repository.Delete(id);
    }
}