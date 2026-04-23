using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RealEstateAgency.Application.Interfaces.Services;

namespace RealEstateAgency.Infrastructure.Services;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public CookieService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public void SetRefreshTokenCookie(string token)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Refresh:ExpireDays"]))
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", token, options);
    }
}