using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class VerificationRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IVerificationRepository
{
    public async Task<Guid> Insert(Verification verification)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Verifications.AddAsync(verification);
        await ctx.SaveChangesAsync();
        return res.Entity.Id;
    }
    
    public async Task<bool> Delete(Verification verification)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            ctx.Verifications.Remove(verification);
            await ctx.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}