using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class QuestionRepository(RealEstateContext ctx) : IQuestionRepository
{
    public async Task<Guid> GetQuestionUserIdByAnswerIdAsync(Guid id)
    {
        var questionUserId = await ctx.Answers
            .Where(x => x.Id == id)
            .Select(x => x.QuestionNavigation!.UserId)
            .FirstOrDefaultAsync();
        return questionUserId;
    }

    public async Task<List<QuestionAnswerModelDto>> GetQuestionsAnswersByAnnouncementIdAsync(Guid id)
    {
        var res = await ctx.Questions
            .Where(x => x.AnnouncementId == id)
            .Select(x => new QuestionAnswerModelDto
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
        await ctx.Questions
            .AddAsync(question);
        
        return question.Id;
    }
    
    public async Task<Question?> GetByIdAsync(Guid id)
    {
        var res = await ctx.Questions
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        
        return res;
    }
    
    public async Task<List<QuestionAnswerGridDto>> GetQuestionsAnswersGridAsync()
    {
        var res = await ctx.Questions.Select(x => new QuestionAnswerGridDto
        {
            QuestionId = x.Id,
            TextQuestion = x.Text,
            CreatedAtQuestion = x.CreatedAt,
            CreatedByQuestion = x.UserNavigation!.UserName!,
            AnswerId = x.AnswerNavigation!.Id,
            CreatedAtAnswer = x.AnswerNavigation.CreatedAt,
            TextAnswer = x.AnswerNavigation.Text,
            CreatedByAnswer = x.AnswerNavigation.UserNavigation!.UserName!,
            AnnouncementName = x.AnnouncementNavigation!.StatementNavigation!.Title,
            AnnouncementId = x.AnnouncementId
        })
        .OrderByDescending(x => x.CreatedAtQuestion)
        .ToListAsync();
        
        return res;
    }
    
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        try
        {
            var entity = await ctx.Questions.FindAsync(id);
            if (entity is null)
                return false;
            
            ctx.Questions.Remove(entity);
            return true;
        }
        catch
        {
            return false;
        }
    }
}