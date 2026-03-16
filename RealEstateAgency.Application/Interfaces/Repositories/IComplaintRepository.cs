using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IComplaintRepository
{
    Task<bool> IsUserComplainedByUserIdAsync(Guid userId, Guid offerId);
    Task<List<Complaint>> GetAllComplaintsAsync();
    Task<List<Complaint>> GetAllOpenedComplaintsAsync();
    Task<Guid> InsertAsync(Complaint complaint);
    Task<bool> UpdateAsync(ComplaintDto complaint);
    Task<bool> DeleteByIdAsync(Guid id);
}