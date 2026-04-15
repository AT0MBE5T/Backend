using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class UserPushSubscriptionRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IUserPushSubscriptionRepository
{
    public async Task<List<UserPushSubscription>> GetAllByUserId(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var result = await ctx.UserPushSubscriptions
            .Where(x => x.UserId == userId)
            .ToListAsync();
        return result;
    }
    
    public async Task<Guid> Insert(UserPushSubscription model)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        await ctx.UserPushSubscriptions.AddAsync(model);
        await ctx.SaveChangesAsync();
        return model.Id;
    }
    
    public async Task Delete(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        await ctx.UserPushSubscriptions.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
}