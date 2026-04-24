using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IAnswerRepository
{
    Task<Answer?> GetAnswerByIdAsync(Guid id);
    Task<Guid> InsertAsync(Answer answer);
    Task<bool> DeleteByIdAsync(Guid id);
}