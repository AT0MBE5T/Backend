using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using ApplicationMapper = RealEstateAgency.Application.Mappers.ApplicationMapper;

namespace RealEstateAgency.Application.Services;

public class AnswerService(IAnswerRepository answerRepository, ApplicationMapper mapper, IAuditService auditService,
    IUnitOfWork unitOfWork) : IAnswerService
{
    public async Task<Guid?> InsertAnswerAsync(AnswerDto answerDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var answerEntity = mapper.AnswerDtoToAnswerEntity(answerDto);
            var answerId = await answerRepository.InsertAsync(answerEntity);

            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.AddAnswer),
                UserId = answerDto.UserId,
                Details = $"Answer {answerId} created by {answerDto.UserId}"
            };
            
            await auditService.InsertAudit(auditDto);

            await unitOfWork.CommitAsync();
            return answerId;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            return null;
        }
    }

    public async Task<bool> DeleteByAnswerIdAsync(Guid answerId, Guid userId)
    {
        await unitOfWork.BeginTransactionAsync();

        try
        {
            var answer = await answerRepository.GetAnswerByIdAsync(answerId);
            if (answer == null)
            {
                await unitOfWork.RollbackAsync();
                return false;
            }
        
            var res = await answerRepository.DeleteByIdAsync(answerId);
            if (!res)
            {
                await unitOfWork.RollbackAsync();
                return false;
            }
            
            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.DeleteAnswer),
                UserId = answer.UserId,
                Details = $"Answer {answerId} deleted by {userId}"
            };
            
            await auditService.InsertAudit(auditDto);
            await unitOfWork.CommitAsync();
            
            return res;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            return false;
        }
    }
}