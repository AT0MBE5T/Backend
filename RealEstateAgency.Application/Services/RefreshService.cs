using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Services;

public class RefreshService(
    IRefreshRepository refreshRepository,
    IConfiguration configuration,
    ICookieService cookieService,
    IJwtService jwtService,
    IAuditService auditService) : IRefreshService
{
    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        var result = await refreshRepository.GetUserByRefreshTokenAsync(refreshToken);
        return result;
    }
    
    public async Task<bool> DeleteRefreshTokenAsync(string refreshToken)
    {
        var result = await refreshRepository.DeleteAsync(refreshToken);
        return result;
    }

    public async Task<string> GenerateRefreshToken(Guid userId)
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["Refresh:ExpireDays"])),
            UserId = userId
        };

        var result = await refreshRepository.GenerateRefreshToken(refreshToken);
        return result;
    }

    public async Task<bool> CheckRefreshToken(string token)
    {
        var result = await refreshRepository.CheckRefreshToken(token);
        return result;
    }

    public void SetRefreshToken(string refreshToken)
    {
        cookieService.SetRefreshTokenCookie(refreshToken);
    }
    
    public async Task<AverageResponse<string>> RefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken) || !await CheckRefreshToken(refreshToken))
            return new AverageResponse<string>(string.Empty, "Refresh token is not valid");

        var user = await GetUserByRefreshTokenAsync(refreshToken);
        if (user == null) return new AverageResponse<string>(string.Empty, "User is not valid");

        var newAccessToken = await jwtService.GenerateAccessToken(user);
        return new AverageResponse<string>(newAccessToken, string.Empty);
    }

    public async Task<string> LogoutAsync(string refreshToken)
    {
        var user = await GetUserByRefreshTokenAsync(refreshToken);
        if (user == null) return "User not found";

        var deleted = await DeleteRefreshTokenAsync(refreshToken);
        if (!deleted) return "Could not delete token";

        var auditDto = new AuditDto
        {
            ActionId = Guid.Parse(AuditAction.Logout),
            UserId = user.Id,
            Details = $"User {user.UserName} logged out"
        };
        
        await auditService.InsertAudit(auditDto);

        return string.Empty;
    }
}