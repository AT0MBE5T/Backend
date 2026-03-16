using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IImageRepository
{
    Task<Guid> InsertAsync(Image image);
    Task<bool> UpdateAsync(Image image);
    Task<List<Image>> GetPhotosByPropertyIdAsync(Guid propertyId);
    Task<bool> DeleteAsync(Guid imageId);
    Task<Image?> GetPhotoByIdAsync(Guid id);
}