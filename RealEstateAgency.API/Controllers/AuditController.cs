using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Interfaces.Services;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditController(IAuditService auditService): ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await auditService.GetAllAudits();
        return Ok(result);
    }
}