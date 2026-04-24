using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface ICommentService
{
    Task<List<CommentDto>> GetAllByAnnouncementId(Guid announcementId);
    Task<Guid?> InsertCommentAsync(CommentDto commentDto);
    Task<bool> DeleteByCommentIdAsync(Guid commentId, Guid userId);
    Task<List<CommentGridDto>> GetCommentsGrid();
}