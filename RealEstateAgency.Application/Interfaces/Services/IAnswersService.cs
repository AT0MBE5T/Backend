using RealEstateAgency.Application.Dto;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IAnswersService
{
    Task<Guid?> InsertAnswerAsync(AnswerDto answerDto);
    Task<bool> DeleteByAnswerIdAsync(Guid answerId, Guid userId);
}