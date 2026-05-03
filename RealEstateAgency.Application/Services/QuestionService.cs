using Microsoft.Extensions.Logging;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;
using ApplicationMapper = RealEstateAgency.Application.Mappers.ApplicationMapper;

namespace RealEstateAgency.Application.Services;

public class QuestionService(IQuestionRepository questionRepository, ApplicationMapper mapper,
    IAuditService auditService, IUnitOfWork unitOfWork, ILogger<QuestionService> logger) : IQuestionService
{
    public async Task<List<QuestionAnswerModelDto>> GetQuestionsAnswersByAnnouncementId(Guid id)
    {
        var res = await questionRepository.GetQuestionsAnswersByAnnouncementIdAsync(id);
        return res
            .OrderBy(x => x.CreatedAtQuestion)
            .ToList();
    }

    public async Task<Guid> GetQuestionUserIdByAnswerId(Guid answerId)
    {
        var res = await questionRepository.GetQuestionUserIdByAnswerIdAsync(answerId);
        return res;
    }
    
    public async Task<List<QuestionAnswerGridDto>> GetQuestionsAnswersGrid()
    {
        var res = await questionRepository.GetQuestionsAnswersGridAsync();
        return res;
    }
    
    public async Task<Guid?> InsertQuestionAsync(QuestionDto questionDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var questionEntity = mapper.QuestionDtoToQuestionEntity(questionDto);
            var questionId = await questionRepository.InsertAsync(questionEntity);

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
        catch (Exception ex)
        {
            logger.LogError("Failed to ask a question: {ex}", ex);
            await unitOfWork.RollbackAsync();
            return null;
        }
    }
    
    public async Task<bool> DeleteByQuestionIdAsync(Guid questionId, Guid userId)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var question = await questionRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                await unitOfWork.RollbackAsync();
                return false;
            }
            var res = await questionRepository.DeleteByIdAsync(questionId);
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
        catch (Exception ex)
        {
            logger.LogError("Failed to delete a question: {ex}", ex);
            await unitOfWork.RollbackAsync();
            return false;
        }
    }
}