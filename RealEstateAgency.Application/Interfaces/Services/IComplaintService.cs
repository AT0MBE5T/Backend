using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IComplaintService
{
    Task<bool> IsUserComplained(Guid userId, Guid offerId);
    Task<ComplaintDto?> GetByIdAsync(Guid complaintId);
    Task<List<ComplaintGrid>> GetAllComplaints();
    Task<List<ComplaintGrid>> GetAllOpenedComplaints();
    Task<List<ComplaintGrid>> GetComplaintsByUserId(Guid userId);
    Task<Guid> InsertAsync(ComplaintDto complaint);
    Task<bool> UpdateAsync(ComplaintDto complaint);
    Task<bool> DeleteByIdAsync(Guid id);
}