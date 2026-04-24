using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IStatementRepository
{
    Task<Guid> InsertAsync(Statement statement);
    Task<bool> UpdateAsync(Guid id, Statement newStatement);
    Task<Statement?> GetByIdAsync(Guid id);
}