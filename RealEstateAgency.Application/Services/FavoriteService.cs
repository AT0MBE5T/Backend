using Microsoft.Extensions.Logging;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Core.Dtos;
using ApplicationMapper = RealEstateAgency.Application.Mappers.ApplicationMapper;

namespace RealEstateAgency.Application.Services;

public class FavoriteService(
    IFavoriteRepository favoriteRepository,
    IPaymentService paymentService,
    ApplicationMapper applicationMapper,
    ILogger<FavoriteService> logger) : IFavoriteService
{
    public async Task<AnnouncementsShortAndPagesDto> GetFavoritesByUserId(Guid userId, int page, int limit)
    {
        var result = await  favoriteRepository.GetFavoritesByUserId(userId, page, limit);
        return result;
    }
    
    public async Task<bool> IsInFavoriteAsync(FavoriteDto dto)
    {
        var result = await favoriteRepository.IsFavorited(dto.UserId, dto.AnnouncementId);
        return result;
    }

    public async Task<string> AddFavoriteAsync(FavoriteDto dto)
    {
        try
        {
            var isPaid = await paymentService.IsExistByAnnouncementIdAsync(dto.AnnouncementId);

            if (isPaid)
                return "Announcement already paid";

            var favoriteDto = new FavoriteDto
            {
                UserId = dto.UserId,
                AnnouncementId = dto.AnnouncementId,
                CreatedAt = DateTime.UtcNow
            };

            var isInFavorite = await IsInFavoriteAsync(dto);

            if (isInFavorite)
                return "Already in favorite";

            var model = applicationMapper.MapFavoriteDtoToEntity(dto);
            var result = await favoriteRepository.AddAsync(model);
            return result
                ? string.Empty
                : "Addition failed";
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to add to favorite: {ex}", ex);
            return "Failed to add to favorite";
        }
    }

    public async Task<string> DeleteByDtoAsync(FavoriteDto dto)
    {
        try{
            var isPaid = await paymentService.IsExistByAnnouncementIdAsync(dto.AnnouncementId);
            
            if (isPaid)
                return "Announcement already paid";
            
            var result = await favoriteRepository.DeleteByIdAsync(dto.UserId, dto.AnnouncementId);
            return string.Empty;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to remove from favorite: {ex}", ex);
            return "Failed to remove from favorite";
        }
    }
}