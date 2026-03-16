using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RealEstateAgency.API.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Services;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;
    private readonly IImageRepository _repository;
    private readonly IAccountService _accountService;

    public ImageService(IImageRepository repository, IConfiguration config,  IAccountService accountService)
    {
        _repository = repository;
        _accountService = accountService;
        
        var acc = new Account(
            config["CloudinarySettings:CloudName"],
            config["CloudinarySettings:ApiKey"],
            config["CloudinarySettings:ApiSecret"]
        );
        _cloudinary = new Cloudinary(acc);
    }
    
    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        if (file.Length > 0) {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams {
                File = new FileDescription(file.FileName, stream)
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        return uploadResult;
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
    
    public async Task<bool> UpdateAsync(Guid imageId, byte[] bytes)
    {
        return false;
        // try
        // {
        //     var res = await _repository.UpdateAsync(imageId, bytes);
        //     return res;
        // }
        // catch
        // {
        //     return false;
        // }
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
    
    public async Task<bool> DeleteAvatarAsync(Guid userId)
    {
        try
        {
            var userDto =  await _accountService.GetUserDtoById(userId);
            
            if (userDto is null)
                return false;
            
            var deletionResult = await DeleteImageAsync(userDto.PublicId);
            return true;
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