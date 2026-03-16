using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Infrastructure.Repositories;

namespace RealEstateAgency.Application.Services;

public class ComplaintService(IComplaintRepository complaintRepository, ApplicationMapper applicationMapper) : IComplaintService
{
    public async Task<bool> IsUserComplained(Guid userId, Guid offerId)
    {
        var result = await complaintRepository.IsUserComplainedByUserIdAsync(userId, offerId);
        return result;
    }

    public async Task<List<ComplaintDto>> GetAllComplaints()
    {
        var result = await complaintRepository.GetAllComplaintsAsync();
        return result.Select(applicationMapper.MapComplaintToDto).ToList();
    }

    public async Task<List<ComplaintDto>> GetAllOpenedComplaints()
    {
        var result = await complaintRepository.GetAllOpenedComplaintsAsync();
        return result.Select(applicationMapper.MapComplaintToDto).ToList();
    }

    public async Task<Guid> InsertAsync(ComplaintDto complaint)
    {
        var isAlreadyComplained = await IsUserComplained(complaint.UserId, complaint.AnnouncementId);
        
        if (isAlreadyComplained)
            return Guid.Empty;
        
        var model = applicationMapper.MapComplaintDtoToEntity(complaint);
        var result = await complaintRepository.InsertAsync(model);
        return result;
    }

    public async Task<bool> UpdateAsync(ComplaintDto complaint)
    {
        var result = await complaintRepository.UpdateAsync(complaint);
        return result;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var result = await complaintRepository.DeleteByIdAsync(id);
        return result;
    }
}