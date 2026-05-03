using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class AnswerRepository(RealEstateContext ctx) : IAnswerRepository
{
    public async Task<Answer?> GetAnswerByIdAsync(Guid id)
    {
        
        return await ctx.Answers
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }
    
    public async Task<Guid> InsertAsync(Answer answer)
    {
        await ctx.Answers.AddAsync(answer);
        return answer.Id;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var entity = await ctx.Answers.FindAsync(id);
        if (entity == null)
            return false;
        
        ctx.Answers.Remove(entity);
        return true;
    }
}