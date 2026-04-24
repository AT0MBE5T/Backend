using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IComplaintRepository
{
    Task<bool> IsUserComplainedByUserIdAsync(Guid userId, Guid offerId);
    Task<Complaint?> GetByIdAsync(Guid complaintId);
    Task<List<ComplaintGridDto>> GetComplaintsByUserId(Guid userId);
    Task<List<ComplaintGridDto>> GetAllComplaintsAsync();
    Task<List<ComplaintGridDto>> GetAllOpenedComplaintsAsync();
    Task<Guid> InsertAsync(Complaint complaint);
    Task<bool> UpdateAsync(Complaint complaint);
}