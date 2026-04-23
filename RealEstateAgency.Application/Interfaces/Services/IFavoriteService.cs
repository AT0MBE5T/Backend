using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IFavoriteService
{
    Task<AnnouncementsShortAndPages> GetSearchDataAsync(Guid userId, string text, List<string> filtersId,
        int sortId, int pageNumber, int pageSize);

    Task<bool> IsInFavoriteAsync(FavoriteDto dto);
    Task<AnnouncementsShortAndPages> GetFavoritesByUserId(Guid userId, int page, int limit);
    Task<string> AddComplaintAsync(FavoriteDto dto);
    Task<string> DeleteByDtoAsync(FavoriteDto dto);
}