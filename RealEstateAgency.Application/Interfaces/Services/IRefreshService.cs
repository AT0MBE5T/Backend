using Microsoft.AspNetCore.Http;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IRefreshService
{
    Task<string> GenerateRefreshToken(Guid userId);
    void SetRefreshToken(string refreshToken);
    Task<AverageResponseDto<string>> RefreshTokenAsync(string refreshToken);
    Task<string> LogoutAsync(string refreshToken);
}