using RealEstateAgency.Application.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IStatementService
{
    Task<Guid?> AddStatementAsync(StatementDto statementDto);
    Task<bool> UpdateStatementAsync(Guid statementId, Guid userId, StatementDto statementDto);
    Task<StatementDto?> GetStatementByIdAsync(Guid statementId);
}