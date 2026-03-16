using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface ICommentsRepository
{
    Task<List<Comment>> GetAllByAnnouncementIdAsync(Guid id);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<Comment?> GetCommentByIdAsync(Guid id);
    Task<Guid> InsertAsync(Comment comment);
}