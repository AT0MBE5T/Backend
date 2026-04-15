using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.DTO;
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
    
    public async Task<List<AuditGrid>> GetAll()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.AuHistories.Select(x => new AuditGrid
        {
            Id =  x.Id,
            CreatedAt = x.CreatedAt,
            ActionId =  x.ActionId,
            UserId =  x.UserId,
            Details =  x.Details,
            ActionName = x.ActionNavigation.Name,
            UserName = x.UserNavigation.UserName
        }).ToListAsync();
        return res;
    }
}