using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IJwtService
{
    Task<string> GenerateAccessToken(User user);
}