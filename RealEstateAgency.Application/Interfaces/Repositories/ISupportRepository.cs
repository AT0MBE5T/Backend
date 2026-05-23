using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Infrastructure.Repositories;

public interface ISupportRepository
{
    Task<bool> IsUserHasUnclosedSupportAsync(Guid userId);
    Task<Support?> GetByIdAsync(Guid supportId);
    Task<List<SupportGridDto>> GetAllSupportsAsync();
    Task<SupportGridDto?> GetSupportGridByIdAsync(Guid supportId);
    Task<List<SupportGridDto>> GetAllOpenedSupportsAsync();
    Task<Guid> InsertAsync(Support support);
    Task<bool> AdminJoinSupportAsync(Guid supportId, Guid adminId);
    Task<bool> CloseSupportAsync(Guid supportId);
}