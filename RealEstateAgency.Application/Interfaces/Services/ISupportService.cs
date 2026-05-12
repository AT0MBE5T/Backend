using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Services;

public interface ISupportService
{
    Task<bool> IsUserHasUnclosedSupport(Guid userId);
    Task<List<SupportGridDto>> GetAllSupports();
    Task<SupportDto?> GetByIdAsync(Guid complaintId);
    Task<List<SupportGridDto>> GetAllOpenedSupports();
    Task<Guid> InsertAsync(SupportDto support);
    Task<Guid?> AdminJoinAsync(Guid supportId, Guid adminId);
    Task<bool> CloseAsync(Guid supportId, Guid userId);
}