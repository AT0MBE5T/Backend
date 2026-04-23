using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Interfaces.Services;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RefreshController(
    IRefreshService refreshService): ControllerBase
{
    // [HttpPost("refresh")]
    // public async Task<IActionResult> Refresh()
    // {
    //     var refreshToken = Request.Cookies["refreshToken"];
    //     if (refreshToken == null)
    //     {
    //         return Unauthorized();
    //     }
    //     
    //     if (!await refreshService.CheckRefreshToken(refreshToken))
    //     {
    //         Response.Cookies.Delete("refreshToken");
    //         return Unauthorized();
    //     }
    //     
    //     var user = await refreshService.GetUserByRefreshTokenAsync(refreshToken);
    //     if (user == null)
    //     {
    //         return Unauthorized();
    //     }
    //
    //
    //     var accessToken = await jwtService.GenerateAccessToken(user);
    //
    //     return Ok(new { token = accessToken });
    // }
    //
    // [HttpGet("logout")]
    // public async Task<IActionResult> Logout()
    // {
    //     var refreshToken = Request.Cookies["refreshToken"];
    //     if (refreshToken == null || !await refreshService.DeleteRefreshTokenAsync(refreshToken))
    //     {
    //         return BadRequest();
    //     }
    //     
    //     var user = await refreshService.GetUserByRefreshTokenAsync(refreshToken);
    //     if (user == null)
    //     {
    //         return Unauthorized();
    //     }
    //     
    //     Response.Cookies.Delete("refreshToken");
    //
    //     var auditDto = new AuditDto
    //     {
    //         ActionId = Guid.Parse(AuditAction.Logout),
    //         UserId = user.Id,
    //         Details = $"User {user.UserName} logged out"
    //     };
    //     
    //     await auditService.InsertAudit(auditDto);
    //     
    //     return Ok();
    // }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var result = await refreshService.RefreshTokenAsync(refreshToken ?? string.Empty);

        if (result.Error == string.Empty) return Ok(new { token = result.Value });
        Response.Cookies.Delete("refreshToken");
        return Unauthorized(result.Error);

    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken)) return BadRequest();

        var result = await refreshService.LogoutAsync(refreshToken);
        Response.Cookies.Delete("refreshToken");

        return result == string.Empty
            ? Ok()
            : BadRequest(result);
    }
}