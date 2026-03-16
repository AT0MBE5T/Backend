using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IAnnouncementRepository
{
    Task<List<Announcement>> GetAllPaginatedAsync(int pageNumber, int pageSize);
    Task<Guid> InsertAsync(Announcement announcement);
    Task<bool> UpdateAsync(Guid id, Announcement announcement);
    Task<bool> DeleteAsync(Guid id);
    Task<Announcement?> GetAnnouncementById(Guid id);
    Task<Verification?> GetVerificationAsync(Guid id);
    Task<AnnouncementFull?> GetAnnouncementFullById(Guid id, Guid? userId);
    Task<int> GetAmount();
    Task<AnnouncementsShortAndPages> GetSearchData(string text, List<string> filtersId, int sortId, int pageNumber, int pageSize, Guid? userId);
    Task<bool> SetClosedAt(Guid id);
    Task<AnnouncementsShortAndPages> GetPlacedByUserId(Guid userId, int pageNumber, int pageSize);
    Task<AnnouncementsShortAndPages> GetSoldByUserId(Guid userId, int pageNumber, int pageSize);
    Task<AnnouncementsShortAndPages> GetBoughtByUserId(Guid userId, int pageNumber, int pageSize);
    Task<int> GetTotalAnnouncements();
    Task<decimal> GetTotalIncome();
    Task<GeneralTopDeal?> GetTopDeal();
    Task<List<GeneralTopRealtors>> GetTopRealtors(int top);
    Task<List<GeneralTopProperty>> GetTopPropertyTypes(int top);
    Task<List<GeneralTopClient>> GetTopClients(int top);
    Task<byte[]> GetBytesByAnnouncementIdAsync(Guid announcementId);
    Task<Guid> GetImageIdByAnnouncementIdAsync(Guid announcementId);
    Task<Guid?> GetPropertyIdByAnnouncementIdAsync(Guid announcementId);
    Task<Guid?> GetStatementIdByAnnouncementIdAsync(Guid announcementId);
    Task<AnnouncementShort?> GetAnnouncementShortByOfferId(Guid offerId, Guid userId);
}