using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using RealEstateAgency.API.Dto;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IImageService
{
    Task<Guid> InsertAsync(ImageDto imageDto);
    Task<bool> UpdateAsync(Guid imageId, byte[] bytes);
    Task<bool> UpdateOrderAsync(Guid imageId, int currentIndex);
    Task<bool> DeleteAsync(Guid imageId);
    Task<DeletionResult> DeleteImageAsync(string publicId);
    Task<List<PhotoDto>> GetPhotosByPropertyIdAsync(Guid propertyId);
    Task<ImageUploadResult> UploadImageAsync(IFormFile file);
    Task<bool> DeleteAvatarAsync(Guid userId);
}