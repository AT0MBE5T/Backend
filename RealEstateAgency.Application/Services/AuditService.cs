using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Services;

public class AuditService(IAuditRepository auditRepository, ApplicationMapper mapper) : IAuditService
{
    public async Task InsertAudit(AuditDto auditDto)
    {
        var auditEntity = mapper.AuditDtoToAuHistory(auditDto);
        await auditRepository.InsertAsync(auditEntity);
    }
    
    public async Task<List<AuditGrid>> GetAllAudits()
    {
        var result = await auditRepository.GetAll();
        return result;
    }
}