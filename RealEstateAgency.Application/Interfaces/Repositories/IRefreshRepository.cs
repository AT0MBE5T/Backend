using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IRefreshRepository
{
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    Task<bool> DeleteAsync(string refreshToken);
    Task<string> GenerateRefreshToken(RefreshToken refreshToken);
    Task<bool> CheckRefreshToken(string token);
}