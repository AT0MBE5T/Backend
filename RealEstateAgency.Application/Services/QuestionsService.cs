using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Services;

public class QuestionsService(IQuestionsRepository questionsRepository, ApplicationMapper mapper,
    IAuditService auditService, IUnitOfWork unitOfWork) : IQuestionsService
{
    public async Task<List<QuestionAnswerModel>> GetQuestionsAnswersByAnnouncementId(Guid id)
    {
        var res = await questionsRepository.GetQuestionsAnswersByAnnouncementIdAsync(id);
        return res
            .OrderBy(x => x.CreatedAtQuestion)
            .ToList();
    }
    
    public async Task<Guid?> InsertQuestionAsync(QuestionDto questionDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var questionEntity = mapper.QuestionDtoToQuestionEntity(questionDto);
            var questionId = await questionsRepository.InsertAsync(questionEntity);

            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.AddQuestion),
                UserId = questionDto.UserId,
                Details = $"Question {questionId} created by {questionDto.UserId}"
            };
            
            await auditService.InsertAudit(auditDto);
            await unitOfWork.CommitAsync();
            return questionId;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            return null;
        }
    }
    
    public async Task<bool> DeleteByQuestionIdAsync(Guid questionId, Guid userId)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var question = await questionsRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                await unitOfWork.RollbackAsync();
                return false;
            }
            var res = await questionsRepository.DeleteByIdAsync(questionId);
            if (!res)
            {
                await unitOfWork.RollbackAsync();
                return false;
            }

            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.DeleteQuestion),
                UserId = question.UserId,
                Details = $"Question {questionId} deleted by {userId}"
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