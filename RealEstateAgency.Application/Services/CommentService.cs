using Microsoft.Extensions.Logging;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Dtos;
using ApplicationMapper = RealEstateAgency.Application.Mappers.ApplicationMapper;

namespace RealEstateAgency.Application.Services;

public class CommentService(ICommentRepository commentRepository, IAuditService auditService,
    ApplicationMapper mapper, IUnitOfWork unitOfWork, ILogger<CommentService> logger) : ICommentService
{
    public async Task<List<CommentDto>> GetAllByAnnouncementId(Guid announcementId)
    {
        var res = await commentRepository.GetAllByAnnouncementIdAsync(announcementId);
        return res;
    }
    
    public async Task<Guid?> InsertCommentAsync(CommentDto commentDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var commentEntity = mapper.CommentDtoToCommentEntity(commentDto);
            var commentId = await commentRepository.InsertAsync(commentEntity);

            if (commentId == Guid.Empty)
            {
                return Guid.Empty;
            }

            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.AddComment),
                UserId = commentEntity.UserId,
                Details = $"Comment {commentId} added by {commentDto.UserId}"
            };
            
            await auditService.InsertAudit(auditDto);
            await unitOfWork.CommitAsync();
            return commentId;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to create a comment: {ex}", ex);
            await unitOfWork.RollbackAsync();
            return null;
        }
    }
    
    public async Task<bool> DeleteByCommentIdAsync(Guid commentId, Guid userId)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var comment = await commentRepository.GetCommentByIdAsync(commentId);

            if (comment == null)
            {
                return false;
            }

            var res = await commentRepository.DeleteByIdAsync(commentId);
            if (!res)
            {
                await unitOfWork.RollbackAsync();
                return false;
            }

            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.DeleteComment),
                UserId = comment.UserId,
                Details = $"Comment {commentId} deleted by {userId}"
            };

            await auditService.InsertAudit(auditDto);
            await unitOfWork.CommitAsync();

            return res;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to delete a comment: {ex}", ex);
            await unitOfWork.RollbackAsync();
            return false;
        }
    }

    public async Task<List<CommentGridDto>> GetCommentsGrid()
    {
        var res = await commentRepository.GetCommentsGridAsync();
        return res;
    }
}