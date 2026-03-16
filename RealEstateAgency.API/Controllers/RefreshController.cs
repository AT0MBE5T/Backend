using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Services;
using RealEstateAgency.Application.Utils;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RefreshController(IJwtService jwtService, IRefreshService refreshService, IAuditService auditService): ControllerBase
{
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken == null)
        {
            return Unauthorized();
        }
        
        if (!await refreshService.CheckRefreshToken(refreshToken))
        {
            Response.Cookies.Delete("refreshToken");
            return Unauthorized();
        }
        
        var user = await refreshService.GetUserByRefreshTokenAsync(refreshToken);
        if (user == null)
        {
            return Unauthorized();
        }


        var accessToken = await jwtService.GenerateAccessToken(user);

        return Ok(new { token = accessToken });
    }
    
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken == null || !await refreshService.DeleteRefreshTokenAsync(refreshToken))
        {
            return BadRequest();
        }
        
        var user = await refreshService.GetUserByRefreshTokenAsync(refreshToken);
        if (user == null)
        {
            return Unauthorized();
        }
        
        Response.Cookies.Delete("refreshToken");

        var auditDto = new AuditDto
        {
            ActionId = Guid.Parse(AuditAction.Logout),
            UserId = user.Id,
            Details = $"User {user.UserName} logged out"
        };
        
        await auditService.InsertAudit(auditDto);
        
        return Ok();
    }
}