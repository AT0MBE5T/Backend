using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Services;

public class CommentsService(ICommentsRepository commentsRepository, IAuditService auditService,
    ApplicationMapper mapper, IUnitOfWork unitOfWork) : ICommentsService
{
    public async Task<List<CommentDto>> GetAllByAnnouncementId(Guid announcementId)
    {
        var res = await commentsRepository.GetAllByAnnouncementIdAsync(announcementId);
        return res;
    }
    
    public async Task<Guid?> InsertCommentAsync(CommentDto commentDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var commentEntity = mapper.CommentDtoToCommentEntity(commentDto);
            var commentId = await commentsRepository.InsertAsync(commentEntity);

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
        catch
        {
            await unitOfWork.RollbackAsync();
            return null;
        }
    }
    
    public async Task<bool> DeleteByCommentIdAsync(Guid commentId)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var comment = await commentsRepository.GetCommentByIdAsync(commentId);

            if (comment == null)
            {
                return false;
            }

            var userId = comment.UserId;

            var res = await commentsRepository.DeleteByIdAsync(commentId);
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
        catch
        {
            await unitOfWork.RollbackAsync();
            return false;
        }
    }

    public async Task<List<CommentGrid>> GetCommentsGrid()
    {
        var res = await commentsRepository.GetCommentsGridAsync();
        return res;
    }
}