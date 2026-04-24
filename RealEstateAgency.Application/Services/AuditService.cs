using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Core.Dtos;
using ApplicationMapper = RealEstateAgency.Application.Mappers.ApplicationMapper;

namespace RealEstateAgency.Application.Services;

public class AuditService(IAuditRepository auditRepository, ApplicationMapper mapper) : IAuditService
{
    public async Task InsertAudit(AuditDto auditDto)
    {
        var auditEntity = mapper.AuditDtoToAuHistory(auditDto);
        await auditRepository.InsertAsync(auditEntity);
    }
    
    public async Task<List<AuditGridDto>> GetAllAudits()
    {
        var result = await auditRepository.GetAll();
        return result;
    }
}