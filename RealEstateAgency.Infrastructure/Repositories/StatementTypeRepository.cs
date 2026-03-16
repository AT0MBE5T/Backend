using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class StatementTypeRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IStatementTypeRepository
{
    public async Task<Guid> GetIdByNameAsync(string name)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res= await ctx.StatementTypes
            .Where(p => p.Name == name)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();

        return res;
    }
    
    public async Task<List<StatementType>> GetAllAsync()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res= await ctx.StatementTypes.ToListAsync();
        return res;
    }
}