using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using ApplicationMapper = RealEstateAgency.Application.Mappers.ApplicationMapper;

namespace RealEstateAgency.Application.Services;

public class StatementService(IStatementRepository repository, ApplicationMapper mapper,
    IAuditService auditService, IUnitOfWork unitOfWork) : IStatementService
{
    public async Task<Guid?> AddStatementAsync(StatementDto statementDto)
    {
        try
        {
            var statementEntity = mapper.StatementDtoToStatementEntity(statementDto);
            var statementId = await repository.InsertAsync(statementEntity);
            return statementId;
        }
        catch
        {
            return null;
        }
    }
    
    public async Task<bool> UpdateStatementAsync(Guid statementId, Guid userId, StatementDto statementDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var statementEntity = mapper.StatementDtoToStatementEntity(statementDto);
            var result = await repository.UpdateAsync(statementId, statementEntity);
            
            if (!result)
            {
                await unitOfWork.RollbackAsync();
                return false;
            }

            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.UpdateStatement),
                UserId = userId,
                Details = $"Statement {statementId} updated by {userId}"
            };
            
            await auditService.InsertAudit(auditDto);
            await unitOfWork.CommitAsync();
            return true;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
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