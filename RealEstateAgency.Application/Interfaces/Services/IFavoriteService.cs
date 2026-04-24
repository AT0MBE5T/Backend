using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IFavoriteService
{
    Task<bool> IsInFavoriteAsync(FavoriteDto dto);
    Task<AnnouncementsShortAndPagesDto> GetFavoritesByUserId(Guid userId, int page, int limit);
    Task<string> AddComplaintAsync(FavoriteDto dto);
    Task<string> DeleteByDtoAsync(FavoriteDto dto);
}