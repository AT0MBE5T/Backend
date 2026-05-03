using Microsoft.Extensions.Logging;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using ApplicationMapper = RealEstateAgency.Application.Mappers.ApplicationMapper;

namespace RealEstateAgency.Application.Services;

public class StatementService(IStatementRepository repository, ApplicationMapper mapper,
    IAuditService auditService, ILogger<StatementService> logger) : IStatementService
{
    public async Task<Guid?> AddStatementAsync(StatementDto statementDto)
    {
        try
        {
            var statementEntity = mapper.StatementDtoToStatementEntity(statementDto);
            var statementId = await repository.InsertAsync(statementEntity);
            return statementId;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to create a statement: {ex}", ex);
            return null;
        }
    }
    
    public async Task<bool> UpdateStatementAsync(Guid statementId, Guid userId, StatementDto statementDto)
    {
        try
        {
            var statementEntity = mapper.StatementDtoToStatementEntity(statementDto);
            var result = await repository.UpdateAsync(statementId, statementEntity);
            
            if (!result)
            {
                return false;
            }

            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.UpdateStatement),
                UserId = userId,
                Details = $"Statement {statementId} updated by {userId}"
            };
            
            await auditService.InsertAudit(auditDto);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to update a statement: {ex}", ex);
            return false;
        }
    }

    public async Task<StatementDto?> GetStatementByIdAsync(Guid statementId)
    {
        var statement = await repository.GetByIdAsync(statementId);
        if (statement == null)
        {
            return null;
        }
        var statementDto = mapper.StatementEntityToStatementDto(statement);
        return statementDto;
    }
}