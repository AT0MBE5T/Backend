using RealEstateAgency.Application.Dto;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IComplaintService
{
    Task<bool> IsUserComplained(Guid userId, Guid offerId);
    Task<List<ComplaintDto>> GetAllComplaints();
    Task<List<ComplaintDto>> GetAllOpenedComplaints();
    Task<Guid> InsertAsync(ComplaintDto complaint);
    Task<bool> UpdateAsync(ComplaintDto complaint);
    Task<bool> DeleteByIdAsync(Guid id);
}