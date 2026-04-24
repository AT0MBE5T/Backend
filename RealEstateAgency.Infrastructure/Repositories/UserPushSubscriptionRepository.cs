using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class UserPushSubscriptionRepository(RealEstateContext ctx) : IUserPushSubscriptionRepository
{
    public async Task<List<UserPushSubscription>> GetAllByUserId(Guid userId)
    {
        var result = await ctx.UserPushSubscriptions
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync();
        return result;
    }
    
    public async Task<Guid> Insert(UserPushSubscription model)
    {
        await ctx.UserPushSubscriptions.AddAsync(model);
        await ctx.SaveChangesAsync();
        return model.Id;
    }
    
    public async Task Delete(Guid id)
    {
        await ctx.UserPushSubscriptions.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
}