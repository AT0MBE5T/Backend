using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface ICommentRepository
{
    Task<List<CommentDto>> GetAllByAnnouncementIdAsync(Guid id);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<Comment?> GetCommentByIdAsync(Guid id);
    Task<List<CommentGridDto>> GetCommentsGridAsync();
    Task<Guid> InsertAsync(Comment comment);
}