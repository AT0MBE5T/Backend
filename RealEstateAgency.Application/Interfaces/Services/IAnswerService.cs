using RealEstateAgency.Application.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IAnswerService
{
    Task<Guid?> InsertAnswerAsync(AnswerDto answerDto);
    Task<bool> DeleteByAnswerIdAsync(Guid answerId, Guid userId);
}