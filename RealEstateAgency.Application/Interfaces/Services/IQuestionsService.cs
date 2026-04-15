using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IQuestionsService
{
    Task<List<QuestionAnswerModel>> GetQuestionsAnswersByAnnouncementId(Guid id);
    Task<Guid> GetQuestionUserIdByAnswerId(Guid answerId);
    Task<Guid?> InsertQuestionAsync(QuestionDto questionDto);
    Task<bool> DeleteByQuestionIdAsync(Guid questionId, Guid userId);
    Task<List<QuestionAnswerGrid>> GetQuestionsAnswersGrid();
}