using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class StatementRepository(RealEstateContext ctx) : IStatementRepository
{
    public async Task<Guid> InsertAsync(Statement statement)
    {
        await ctx.Statements.AddAsync(statement);
        return statement.Id;
    }
    
    public async Task<bool> UpdateAsync(Guid id, Statement newStatement)
    {
        var statement =  await ctx.Statements.FindAsync(id);

        if (statement == null)
        {
            return false;
        }
    
        statement.Content = newStatement.Content;
        statement.CreatedAt = newStatement.CreatedAt;
        statement.Price = newStatement.Price;
        statement.PropertyId = newStatement.PropertyId;
        statement.StatementTypeId = newStatement.StatementTypeId;
        statement.Title = newStatement.Title;
        
        return true;
    }
    
    public async Task<Statement?> GetByIdAsync(Guid id)
    {
        var data = await ctx.Statements
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        return data;
    }
}