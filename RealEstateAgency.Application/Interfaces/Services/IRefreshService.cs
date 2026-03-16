using Microsoft.AspNetCore.Http;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IRefreshService
{
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    Task<bool> DeleteRefreshTokenAsync(string refreshToken);
    Task<string> GenerateRefreshToken(Guid userId);
    Task<bool> CheckRefreshToken(string token);
    CookieOptions GetCookieOptions();
}