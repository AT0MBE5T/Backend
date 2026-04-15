using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IComplaintRepository
{
    Task<bool> IsUserComplainedByUserIdAsync(Guid userId, Guid offerId);
    Task<Complaint?> GetByIdAsync(Guid complaintId);
    Task<List<ComplaintGrid>> GetComplaintsByUserId(Guid userId);
    Task<List<ComplaintGrid>> GetAllComplaintsAsync();
    Task<List<ComplaintGrid>> GetAllOpenedComplaintsAsync();
    Task<Guid> InsertAsync(Complaint complaint);
    Task<bool> UpdateAsync(Complaint complaint);
    Task<bool> DeleteByIdAsync(Guid id);
}