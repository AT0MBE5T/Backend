using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IAuditService
{
    Task InsertAudit(AuditDto auditDto);
    Task<List<AuditGridDto>> GetAllAudits();
}