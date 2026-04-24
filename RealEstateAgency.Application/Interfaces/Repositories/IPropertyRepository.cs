using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IPropertyRepository
{
    Task<Guid> InsertAsync(Property property);
    Task<bool> UpdateAsync(Guid id, Property newProperty);
    Task<Property?> GetByIdAsync(Guid id);
}