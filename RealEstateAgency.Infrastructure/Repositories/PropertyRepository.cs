using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class PropertyRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IPropertyRepository
{
    public async Task<Guid> InsertAsync(Property property)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        await ctx.Properties.AddAsync(property);
        await ctx.SaveChangesAsync();
        return property.Id;
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        try
        {
            await ctx.Properties.Where(x => x.Id == id).ExecuteDeleteAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    // TODO: imageService
    public async Task<bool> UpdateAsync(Guid id, Property newProperty)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();

        try
        {
            var property = await ctx.Properties.FindAsync(id);

            if (property == null)
            {
                return false;
            }
            
            property.Area = newProperty.Area;
            property.Description = newProperty.Description;
            property.Floors = newProperty.Floors;
            //property.ImageId = newProperty.ImageId;
            property.Location = newProperty.Location;
            property.PropertyTypeId = newProperty.PropertyTypeId;
            property.Rooms = newProperty.Rooms;
            await ctx.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Property?> GetByIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Properties
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return res;
    }
    
    // TODO: imageService
    public async Task<byte[]> GetImageByPropertyIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var property = await ctx.Properties
            .Include(property => property.ImagesNavigation)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        // return property
        //     ?.ImageNavigation
        //     ?.Bytes ?? [];
        return [];
    }
}