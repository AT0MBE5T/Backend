using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class RefreshRepository(IDbContextFactory<RealEstateContext> dbFactory) : IRefreshRepository
{
    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        await using var context = await dbFactory.CreateDbContextAsync();
        return await context.RefreshTokens
            .Where(r => r.Token == refreshToken)
            .Select(r => r.User)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> DeleteAsync(string refreshToken)
    {
        await using var context = await dbFactory.CreateDbContextAsync();
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
        await using var context = await dbFactory.CreateDbContextAsync();

        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();

        return refreshToken.Token;
    }

    public async Task<bool> CheckRefreshToken(string token)
    {
        await using var context = await dbFactory.CreateDbContextAsync();
        var refreshToken = await context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == token);

        return refreshToken != null && refreshToken.Expires >= DateTime.UtcNow;
    }
}