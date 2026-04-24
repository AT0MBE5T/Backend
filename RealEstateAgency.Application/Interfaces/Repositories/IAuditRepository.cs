using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IAuditRepository
{
    Task<Guid> InsertAsync(AuHistory record);
    Task<List<AuditGridDto>> GetAll();
}