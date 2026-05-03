using Microsoft.Extensions.Logging;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Core.Dtos;
using ApplicationMapper = RealEstateAgency.Application.Mappers.ApplicationMapper;

namespace RealEstateAgency.Application.Services;

public class ComplaintService(
        IComplaintRepository complaintRepository,
        ApplicationMapper applicationMapper,
        IPaymentService paymentService,
        ILogger<ComplaintService> logger
    ): IComplaintService
{
    public async Task<bool> IsUserComplained(Guid userId, Guid offerId)
    {
        var result = await complaintRepository.IsUserComplainedByUserIdAsync(userId, offerId);
        return result;
    }

    public async Task<List<ComplaintGridDto>> GetAllComplaints()
    {
        var result = await complaintRepository.GetAllComplaintsAsync();
        return result;
    }
    
    public async Task<ComplaintDto?> GetByIdAsync(Guid complaintId)
    {
        var complaint = await complaintRepository.GetByIdAsync(complaintId);
        
        var result = complaint is not null
            ? applicationMapper.MapComplaintToDto(complaint)
            : null;

        return result;
    }
    
    public async Task<List<ComplaintGridDto>> GetComplaintsByUserId(Guid userId)
    {
        var result = await complaintRepository.GetComplaintsByUserId(userId);
        return result.ToList();
    }

    public async Task<List<ComplaintGridDto>> GetAllOpenedComplaints()
    {
        var result = await complaintRepository.GetAllOpenedComplaintsAsync();
        return result;
    }

    public async Task<Guid> InsertAsync(ComplaintDto complaint)
    {
        try
        {
            var isAlreadyComplained = await IsUserComplained(complaint.UserId, complaint.AnnouncementId);

            if (isAlreadyComplained)
                return Guid.Empty;
        
            var isPaid = await paymentService.IsExistByAnnouncementIdAsync(complaint.AnnouncementId);
        
            if (isPaid)
                return Guid.Empty;
        
            var model = applicationMapper.MapComplaintDtoToEntity(complaint);
            var result = await complaintRepository.InsertAsync(model);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to add a complaint: {ex}", ex);
            return Guid.Empty;
        }
    }

    public async Task<bool> UpdateAsync(ComplaintDto complaint)
    {
        try
        {
            var mapped = applicationMapper.MapComplaintDtoToEntity(complaint);
            var result = await complaintRepository.UpdateAsync(mapped);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to update a complaint: {ex}", ex);
            return false;
        }
    }
}