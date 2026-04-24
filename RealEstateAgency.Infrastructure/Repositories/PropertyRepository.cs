using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class PropertyRepository(RealEstateContext ctx) : IPropertyRepository
{
    public async Task<Guid> InsertAsync(Property property)
    {
        await ctx.Properties.AddAsync(property);
        return property.Id;
    }
    
    public async Task<bool> UpdateAsync(Guid id, Property newProperty)
    {
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
            property.Location = newProperty.Location;
            property.PropertyTypeId = newProperty.PropertyTypeId;
            property.Rooms = newProperty.Rooms;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Property?> GetByIdAsync(Guid id)
    {
        var res = await ctx.Properties
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        return res;
    }
}