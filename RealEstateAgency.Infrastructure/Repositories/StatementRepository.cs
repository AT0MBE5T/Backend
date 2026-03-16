using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class StatementRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IStatementRepository
{
    public async Task<Guid> InsertAsync(Statement statement)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        await ctx.Statements.AddAsync(statement);
        await ctx.SaveChangesAsync();
        return statement.Id;
    }
    
    public async Task<bool> UpdateAsync(Guid id, Statement newStatement)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        try
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

            await ctx.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        try
        {
            await ctx.Statements.Where(x => x.Id == id).ExecuteDeleteAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<Statement?> GetByIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var data = await ctx.Statements.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        return data;
    }
}