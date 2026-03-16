using RealEstateAgency.Application.Dto;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IPropertyService
{
    Task<Guid> AddProperty(PropertyDto announcementDto);
    Task<PropertyDto?> GetPropertyByIdAsync(Guid id);
    Task<bool> UpdatePropertyAsync(Guid propertyId, PropertyDto propertyDto);
    Task<byte[]> GetBytesByPropertyIdAsync(Guid id);
}