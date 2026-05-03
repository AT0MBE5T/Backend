using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class VerificationRepository(RealEstateContext ctx) : IVerificationRepository
{
    public async Task<Guid> Insert(Verification verification)
    {
        var res = await ctx.Verifications.AddAsync(verification);
        return res.Entity.Id;
    }
    
    public async Task<bool> Delete(Verification verification)
    {
        ctx.Verifications.Remove(verification);
        return true;
    }
}