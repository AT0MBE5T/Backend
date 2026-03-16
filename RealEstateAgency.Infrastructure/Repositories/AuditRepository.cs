using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class AuditRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IAuditRepository
{
    public async Task<Guid> InsertAsync(AuHistory record)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        await ctx.AuHistories.AddAsync(record);
        await ctx.SaveChangesAsync();
        return record.Id;
    }
}