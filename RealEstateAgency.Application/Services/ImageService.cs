using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Services;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;
    private readonly IImageRepository _repository;

    public ImageService(IImageRepository repository, IConfiguration config)
    {
        _repository = repository;
        
        var acc = new Account(
            config["CloudinarySettings:CloudName"],
            config["CloudinarySettings:ApiKey"],
            config["CloudinarySettings:ApiSecret"]
        );
        _cloudinary = new Cloudinary(acc);
    }
    
    public async Task<ImageUploadResponseDto> UploadImageAsync(Stream fileStream, string fileName)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            return new ImageUploadResponseDto(
                string.Empty,
                string.Empty,
                uploadResult.Error.Message
            );
        }

        return new ImageUploadResponseDto(
            uploadResult.PublicId,
            uploadResult.SecureUrl.ToString(),
            string.Empty
        );
    }
    
    public async Task<DeletionResult> DeleteImageAsync(string publicId)
    {
        var uploadParams = new DeletionParams(publicId);
        var deletionResult = await _cloudinary.DestroyAsync(uploadParams);

        return deletionResult;
    }
    
    public async Task<Guid> InsertAsync(ImageDto imageDto)
    {
        try
        {
            var newId = Guid.NewGuid();
            var image = new Image
            {
                Id = newId,
                PhotoUrl =  imageDto.PhotoUrl,
                PublicId = imageDto.PublicId,
                PropertyId = imageDto.PropertyId,
                OrderIndex = imageDto.OrderIndex
            };
            await _repository.InsertAsync(image);
            return newId;
        }
        catch
        {
            return Guid.Empty;
        }
    }
    
    public async Task<bool> DeleteAsync(Guid imageId)
    {
        try
        {
            var photo = await _repository.GetPhotoByIdAsync(imageId);

            if (photo is null)
                return false;
            
            var deletionResult = await DeleteImageAsync(photo.PublicId);
            var res = await _repository.DeleteAsync(imageId);
            return res;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<bool> UpdateOrderAsync(Guid imageId, int currentIndex)
    {
        var entity = await _repository.GetPhotoByIdAsync(imageId);

        if (entity is null)
            return false;
        
        entity.OrderIndex = currentIndex;
        var result = await _repository.UpdateAsync(entity);
        return result;
    }
    
    public async Task<List<PhotoDto>> GetPhotosByPropertyIdAsync(Guid propertyId)
    {
        var images = await _repository.GetPhotosByPropertyIdAsync(propertyId);
        var res = images.OrderBy(x => x.OrderIndex).Select(x => new PhotoDto
        {
            Id = x.Id,
            Url = x.PhotoUrl
        })
        .ToList();
        return res;
    }
}