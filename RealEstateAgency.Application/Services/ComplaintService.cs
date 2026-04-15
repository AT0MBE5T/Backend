using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Infrastructure.Repositories;

namespace RealEstateAgency.Application.Services;

public class ComplaintService(
        IComplaintRepository complaintRepository,
        ApplicationMapper applicationMapper,
        IPaymentService paymentService
    ): IComplaintService
{
    public async Task<bool> IsUserComplained(Guid userId, Guid offerId)
    {
        var result = await complaintRepository.IsUserComplainedByUserIdAsync(userId, offerId);
        return result;
    }

    public async Task<List<ComplaintGrid>> GetAllComplaints()
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
    
    public async Task<List<ComplaintGrid>> GetComplaintsByUserId(Guid userId)
    {
        var result = await complaintRepository.GetComplaintsByUserId(userId);
        return result.ToList();
    }

    public async Task<List<ComplaintGrid>> GetAllOpenedComplaints()
    {
        var result = await complaintRepository.GetAllOpenedComplaintsAsync();
        return result;
    }

    public async Task<Guid> InsertAsync(ComplaintDto complaint)
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

    public async Task<bool> UpdateAsync(ComplaintDto complaint)
    {
        var mapped = applicationMapper.MapComplaintDtoToEntity(complaint);
        var result = await complaintRepository.UpdateAsync(mapped);
        return result;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var result = await complaintRepository.DeleteByIdAsync(id);
        return result;
    }
}