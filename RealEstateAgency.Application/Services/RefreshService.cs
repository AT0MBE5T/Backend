using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Services;

public class RefreshService(IRefreshRepository refreshRepository, IConfiguration configuration) : IRefreshService
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

    public CookieOptions GetCookieOptions()
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict, 
            Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["Refresh:ExpireDays"]))
        };
    }
}