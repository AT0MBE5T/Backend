using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class ImageRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IImageRepository
{
    public async Task<Guid> InsertAsync(Image image)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Images.AddAsync(image);
        await ctx.SaveChangesAsync();
        return res.Entity.Id;
    }
    
    public async Task<bool> UpdateAsync(Image image)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var entity = await ctx.Images.FindAsync(image.Id);
        if (entity is null)
            return false;
        
        entity.OrderIndex = image.OrderIndex;
        await ctx.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> DeleteAsync(Guid imageId)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            var res = await ctx.Images
                .Where(x => x.Id == imageId)
                .ExecuteDeleteAsync();
            await ctx.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<List<Image>> GetPhotosByPropertyIdAsync(Guid propertyId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var images = await ctx.Images.Where(x => x.PropertyId == propertyId).ToListAsync();
        return images;
    }
    
    public async Task<Image?> GetPhotoByIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var image = await ctx.Images.FirstOrDefaultAsync(x => x.Id == id);
        return image;
    }
    
    public async Task<string?> GetPublicIdByUserId(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var publicId = await ctx.Users
            .Where(x => x.Id == userId)
            .Select(x => x.PublicAvatarId)
            .FirstOrDefaultAsync();
        
        return publicId;
    }
}