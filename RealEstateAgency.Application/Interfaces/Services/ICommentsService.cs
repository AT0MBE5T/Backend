using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface ICommentsService
{
    Task<List<CommentDto>> GetAllByAnnouncementId(Guid announcementId);
    Task<Guid?> InsertCommentAsync(CommentDto commentDto);
    Task<bool> DeleteByCommentIdAsync(Guid commentId);
    Task<List<CommentGrid>> GetCommentsGrid();
}