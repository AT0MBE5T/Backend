using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IQuestionRepository
{
    Task<List<QuestionAnswerModelDto>> GetQuestionsAnswersByAnnouncementIdAsync(Guid id);
    Task<Guid> GetQuestionUserIdByAnswerIdAsync(Guid id);
    Task<Guid> InsertAsync(Question question);
    Task<Question?> GetByIdAsync(Guid id);
    Task<List<QuestionAnswerGridDto>> GetQuestionsAnswersGridAsync();
    Task<bool> DeleteByIdAsync(Guid id);
}