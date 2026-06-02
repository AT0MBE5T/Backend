using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Interfaces.Services;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RefreshesController(
    IRefreshService refreshService): ControllerBase
{
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        Console.WriteLine($"RefreshToken is {refreshToken}");
        if (string.IsNullOrEmpty(refreshToken)) return BadRequest();
        var result = await refreshService.RefreshTokenAsync(refreshToken ?? string.Empty);

        if (result.Error == string.Empty) return Ok(new { token = result.Value });
        Response.Cookies.Delete("refreshToken");
        return Unauthorized(result.Error);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return Ok();

        await refreshService.LogoutAsync(refreshToken);

        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            Path = "/",
            Secure = true,
            SameSite = SameSiteMode.None
        });

        return Ok();
    }
}