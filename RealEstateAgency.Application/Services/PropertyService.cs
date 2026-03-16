using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;

namespace RealEstateAgency.Application.Services;

public class PropertyService(IPropertyRepository repository, ApplicationMapper applicationMapper) : IPropertyService
{
    public async Task<Guid> AddProperty(PropertyDto announcementDto)
    {
        var entity = applicationMapper.PropertyDtoToPropertyEntity(announcementDto);
        await repository.InsertAsync(entity);
        return announcementDto.Id;
    }

    public async Task<PropertyDto?> GetPropertyByIdAsync(Guid id)
    {
        var property = await repository.GetByIdAsync(id);
        if (property == null)
        {
            return null;
        }
        var propertyDto = applicationMapper.PropertyEntityToPropertyDto(property);
        return propertyDto;
    }
    
    public async Task<bool> UpdatePropertyAsync(Guid propertyId, PropertyDto propertyDto)
    {
        var propertyEntity = applicationMapper.PropertyDtoToPropertyEntity(propertyDto);
        var result = await repository.UpdateAsync(propertyId, propertyEntity);
        return result;
    }
    
    public async Task<byte[]> GetBytesByPropertyIdAsync(Guid id)
    {
        var result = await repository.GetImageByPropertyIdAsync(id);
        return result;
    }
}