using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IQuestionsRepository
{
    Task<List<QuestionAnswerModel>> GetQuestionsAnswersByAnnouncementIdAsync(Guid id);
    Task<Guid> GetQuestionUserIdByAnswerIdAsync(Guid id);
    Task<Guid> InsertAsync(Question question);
    Task<Question?> GetByIdAsync(Guid id);
    Task<List<QuestionAnswerGrid>> GetQuestionsAnswersGridAsync();
    Task<bool> DeleteByIdAsync(Guid id);
}