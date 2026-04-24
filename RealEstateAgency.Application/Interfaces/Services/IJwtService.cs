using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IJwtService
{
    Task<string> GenerateAccessToken(User user);
}