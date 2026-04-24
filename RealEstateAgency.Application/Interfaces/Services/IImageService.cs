using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IImageService
{
    Task<Guid> InsertAsync(ImageDto imageDto);
    Task<bool> UpdateOrderAsync(Guid imageId, int currentIndex);
    Task<bool> DeleteAsync(Guid imageId);
    Task<DeletionResult> DeleteImageAsync(string publicId);
    Task<List<PhotoDto>> GetPhotosByPropertyIdAsync(Guid propertyId);
    Task<ImageUploadResponseDto> UploadImageAsync(Stream fileStream, string fileName);
}