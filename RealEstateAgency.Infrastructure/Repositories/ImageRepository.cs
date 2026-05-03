using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class ImageRepository(RealEstateContext ctx) : IImageRepository
{
    public async Task<Guid> InsertAsync(Image image)
    {
        var res = await ctx.Images.AddAsync(image);
        return res.Entity.Id;
    }
    
    public async Task<bool> UpdateAsync(Image image)
    {
        var entity = await ctx.Images.FindAsync(image.Id);
        if (entity is null)
            return false;
        
        entity.OrderIndex = image.OrderIndex;
        return true;
    }
    
    public async Task<bool> DeleteAsync(Guid imageId)
    {
        var entity = await ctx.Images.FindAsync(imageId);
        if (entity is null)
            return false;
        
        ctx.Images.Remove(entity);
        return true;
    }
    
    public async Task<List<Image>> GetPhotosByPropertyIdAsync(Guid propertyId)
    {
        var images = await ctx.Images
            .AsNoTracking()
            .Where(x => x.PropertyId == propertyId)
            .ToListAsync();
        return images;
    }
    
    public async Task<Image?> GetPhotoByIdAsync(Guid id)
    {
        var image = await ctx.Images
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        return image;
    }
}