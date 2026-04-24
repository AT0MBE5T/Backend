using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IComplaintService
{
    Task<ComplaintDto?> GetByIdAsync(Guid complaintId);
    Task<List<ComplaintGridDto>> GetAllComplaints();
    Task<List<ComplaintGridDto>> GetAllOpenedComplaints();
    Task<List<ComplaintGridDto>> GetComplaintsByUserId(Guid userId);
    Task<Guid> InsertAsync(ComplaintDto complaint);
    Task<bool> UpdateAsync(ComplaintDto complaint);
}