using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IAuditService
{
    Task InsertAudit(AuditDto auditDto);
    Task<List<AuditGrid>> GetAllAudits();
}