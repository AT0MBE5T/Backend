using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IFavoriteRepository
{
    Task<AnnouncementsShortAndPages> GetSearchDataAsync(Guid userId, string text, List<string> filtersId, int sortId, int pageNumber, int pageSize);
    Task<bool> IsFavorited(Guid userId, Guid offerId);
    Task<AnnouncementsShortAndPages> GetFavoritesByUserId(Guid userId, int pageNumber, int pageSize);
    Task<bool> AddAsync(Favorite entity);
    Task<bool> DeleteByIdAsync(Guid userId, Guid announcementId);
}