using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IPropertyRepository
{
    Task<Guid> InsertAsync(Property property);
    Task<bool> UpdateAsync(Guid id, Property newProperty);
    Task<Property?> GetByIdAsync(Guid id);
    Task<byte[]> GetImageByPropertyIdAsync(Guid id);
}