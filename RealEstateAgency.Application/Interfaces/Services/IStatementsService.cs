using RealEstateAgency.Application.Dto;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IStatementsService
{
    Task<Guid?> AddStatementAsync(StatementDto statementDto);
    Task<bool> UpdateStatementAsync(Guid statementId, Guid userId, StatementDto statementDto);
    Task<StatementDto?> GetStatementByIdAsync(Guid statementId);
}