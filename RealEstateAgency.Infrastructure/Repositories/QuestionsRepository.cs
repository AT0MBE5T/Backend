using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class QuestionsRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IQuestionsRepository
{
    public async Task<List<Question>> GetAllByAnnouncementIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Questions
            .Where(x => x.AnnouncementId == id)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<QuestionAnswerModel>> GetQuestionsAnswersByAnnouncementIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Questions
            .Where(x => x.AnnouncementId == id)
            .Select(x => new QuestionAnswerModel
            {
                QuestionId = x.Id,
                TextQuestion = x.Text,
                CreatedAtQuestion = x.CreatedAt,
                CreatedByQuestion = x.UserNavigation!.Name + ' ' + x.UserNavigation.Surname,
                AnswerId = x.AnswerNavigation!.Id,
                CreatedAtAnswer = x.AnswerNavigation.CreatedAt,
                CreatedByAnswer = x.AnswerNavigation.UserNavigation!.Name + ' ' + x.AnswerNavigation.UserNavigation.Surname,
                TextAnswer = x.AnswerNavigation.Text,
                AnnouncementId = id
            })
            .ToListAsync();

        return res;
    }
    
    public async Task<Guid> InsertAsync(Question question)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        await ctx.Questions
            .AddAsync(question);
        await ctx.SaveChangesAsync();
        
        return question.Id;
    }
    
    public async Task<Question?> GetByIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Questions
            .Where(x  => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        
        return res;
    }
    
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            var res = await ctx.Questions
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