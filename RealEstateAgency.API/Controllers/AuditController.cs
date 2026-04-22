using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;

namespace RealEstateAgency.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuditController(IAuditService auditService): ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var result = await auditService.GetAllAudits();
        return Ok(result);
    }
}