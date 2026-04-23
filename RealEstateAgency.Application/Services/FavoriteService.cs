using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Infrastructure.Repositories;

namespace RealEstateAgency.Application.Services;

public class FavoriteService(
    IFavoriteRepository favoriteRepository,
    IPaymentService paymentService,
    ApplicationMapper applicationMapper) : IFavoriteService
{
    public async Task<AnnouncementsShortAndPages> GetSearchDataAsync(Guid userId, string text, List<string> filtersId,
        int sortId, int pageNumber, int pageSize)
    {
        var result = await favoriteRepository.GetSearchDataAsync(userId, text, filtersId, sortId, pageNumber, pageSize);
        return result;
    }

    public async Task<AnnouncementsShortAndPages> GetFavoritesByUserId(Guid userId, int page, int limit)
    {
        var result = await  favoriteRepository.GetFavoritesByUserId(userId, page, limit);
        return result;
    }
    
    public async Task<bool> IsInFavoriteAsync(FavoriteDto dto)
    {
        var result = await favoriteRepository.IsFavorited(dto.UserId, dto.AnnouncementId);
        return result;
    }

    public async Task<string> AddComplaintAsync(FavoriteDto dto)
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

    public async Task<string> DeleteByDtoAsync(FavoriteDto dto)
    {
        var isPaid = await paymentService.IsExistByAnnouncementIdAsync(dto.AnnouncementId);
        
        if (isPaid)
            return "Announcement already paid";
        
        var result = await favoriteRepository.DeleteByIdAsync(dto.UserId, dto.AnnouncementId);
        return string.Empty;
    }
}