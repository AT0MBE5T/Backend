using Microsoft.AspNetCore.Http;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IRefreshService
{
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    Task<bool> DeleteRefreshTokenAsync(string refreshToken);
    Task<string> GenerateRefreshToken(Guid userId);
    Task<bool> CheckRefreshToken(string token);
    void SetRefreshToken(string refreshToken);
    Task<AverageResponse<string>> RefreshTokenAsync(string refreshToken);
    Task<string> LogoutAsync(string refreshToken);
}