using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class StatementTypeRepository(RealEstateContext ctx) : IStatementTypeRepository
{
    public async Task<List<StatementType>> GetAllAsync()
    {
        var res= await ctx.StatementTypes.ToListAsync();
        return res;
    }
}