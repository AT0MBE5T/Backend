using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IAuditRepository
{
    Task<Guid> InsertAsync(AuHistory record);
    Task<List<AuditGrid>> GetAll();
}