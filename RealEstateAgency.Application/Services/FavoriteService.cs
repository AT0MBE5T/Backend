using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Infrastructure.Repositories;

namespace RealEstateAgency.Application.Services;

public class FavoriteService(IFavoriteRepository favoriteRepository, ApplicationMapper applicationMapper) : IFavoriteService
{
    private const int Pagesize = 8;
    
    public async Task<AnnouncementsShortAndPages> GetSearchDataAsync(Guid userId, string text, List<string> filtersId,
        int sortId, int pageNumber, int pageSize)
    {
        var result = await favoriteRepository.GetSearchDataAsync(userId, text, filtersId, sortId, pageNumber, pageSize);
        return result;
    }

    public async Task<AnnouncementsShortAndPages> GetFavoritesByUserId(Guid userId, int page)
    {
        var result = await  favoriteRepository.GetFavoritesByUserId(userId, page, Pagesize);
        return result;
    }
    
    public async Task<bool> IsInFavoriteAsync(FavoriteDto dto)
    {
        var result = await favoriteRepository.IsFavorited(dto.UserId, dto.AnnouncementId);
        return result;
    }

    public async Task<bool> AddAsync(FavoriteDto dto)
    {
        var isAlreadyComplained = await IsInFavoriteAsync(dto);
        
        if (isAlreadyComplained)
            return false;
        
        var model = applicationMapper.MapFavoriteDtoToEntity(dto);
        var result = await favoriteRepository.AddAsync(model);
        return result;
    }

    public async Task<bool> DeleteByDtoAsync(FavoriteDto entity)
    {
        var result = await favoriteRepository.DeleteByIdAsync(entity.UserId, entity.AnnouncementId);
        return result;
    }
}