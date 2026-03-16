using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Application.Utils;

namespace RealEstateAgency.Application.Services;

public class AnswersService(IAnswersRepository answersRepository, ApplicationMapper mapper, IAuditService auditService,
    IUnitOfWork unitOfWork) : IAnswersService
{
    public async Task<Guid?> InsertAnswerAsync(AnswerDto answerDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var answerEntity = mapper.AnswerDtoToAnswerEntity(answerDto);
            var answerId = await answersRepository.InsertAsync(answerEntity);

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
            var answer = await answersRepository.GetAnswerByIdAsync(answerId);
            if (answer == null)
            {
                await unitOfWork.RollbackAsync();
                return false;
            }
        
            var res = await answersRepository.DeleteByIdAsync(answerId);
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