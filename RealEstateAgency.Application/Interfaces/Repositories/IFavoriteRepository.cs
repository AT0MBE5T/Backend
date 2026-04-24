using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IFavoriteRepository
{
    Task<bool> IsFavorited(Guid userId, Guid offerId);
    Task<AnnouncementsShortAndPagesDto> GetFavoritesByUserId(Guid userId, int pageNumber, int pageSize);
    Task<bool> AddAsync(Favorite entity);
    Task<bool> DeleteByIdAsync(Guid userId, Guid announcementId);
}