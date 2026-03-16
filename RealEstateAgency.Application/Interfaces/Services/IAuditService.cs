using RealEstateAgency.Application.Dto;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IAuditService
{
    Task InsertAudit(AuditDto auditDto);
}