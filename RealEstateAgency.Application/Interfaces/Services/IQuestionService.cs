using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IQuestionService
{
    Task<List<QuestionAnswerModelDto>> GetQuestionsAnswersByAnnouncementId(Guid id);
    Task<Guid> GetQuestionUserIdByAnswerId(Guid answerId);
    Task<Guid?> InsertQuestionAsync(QuestionDto questionDto);
    Task<bool> DeleteByQuestionIdAsync(Guid questionId, Guid userId);
    Task<List<QuestionAnswerGridDto>> GetQuestionsAnswersGrid();
}