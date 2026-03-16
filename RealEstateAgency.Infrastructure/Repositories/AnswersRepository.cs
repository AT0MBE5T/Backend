using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class AnswersRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IAnswersRepository
{
    public async Task<Answer?> GetAnswerByIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Answers
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    public async Task<Guid> InsertAsync(Answer answer)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            await ctx.Answers
                .AddAsync(answer);
            await ctx.SaveChangesAsync();
            return answer.Id;
        }
        catch
        {
            return Guid.Empty;
        }
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            var res = await ctx.Answers
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
            return res != 0;
        }
        catch
        {
            return false;
        }
    }
}