using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class RefreshRepository(RealEstateContext context) : IRefreshRepository
{
    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        var result = await context.RefreshTokens
            .AsNoTracking()
            .Where(r => r.Token == refreshToken)
            .Select(r => r.User)
            .FirstOrDefaultAsync();
        return result;
    }
    
    public async Task<bool> DeleteAsync(string refreshToken)
    {
        var tokenEntity = await context.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == refreshToken);

        if (tokenEntity == null)
        {
            return false;
        }
        
        tokenEntity.RevokedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return true;
    }
    
    public async Task<string> GenerateRefreshToken(RefreshToken refreshToken)
    {
        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();

        return refreshToken.Token;
    }

    public async Task<bool> CheckRefreshToken(string token)
    {
        var refreshToken = await context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Token == token);

        return refreshToken != null && refreshToken.Expires >= DateTime.UtcNow;
    }
}