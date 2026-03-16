using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IAuditRepository
{
    Task<Guid> InsertAsync(AuHistory record);
}