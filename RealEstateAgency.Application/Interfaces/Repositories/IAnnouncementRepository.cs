using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IAnnouncementRepository
{
    Task<int> GetTotalViews();
    Task<Guid> GetAuthorOfferIdByQuestionIdAsync(Guid id);
    Task<List<AnnouncementGridDto>> GetAnnouncementsGridAsync();
    Task<Guid> InsertAsync(Announcement announcement);
    Task<bool> UpdateAsync(Guid id, Announcement announcement);
    Task<bool> DeleteAsync(Guid id);
    Task<Announcement?> GetAnnouncementById(Guid id);
    Task<Verification?> GetVerificationAsync(Guid id);
    Task<AnnouncementFullDto?> GetAnnouncementFullById(Guid id, Guid? userId);
    Task<AnnouncementsShortAndPagesDto> GetSearchData(string text, List<string> filtersId, int sortId, int pageNumber, int pageSize, Guid userId);
    Task<bool> SetClosedAt(Guid id);
    Task<AnnouncementsShortAndPagesDto> GetPlacedByUserId(Guid userId, int pageNumber, int pageSize);
    Task<AnnouncementsShortAndPagesDto> GetSoldByUserId(Guid userId, int pageNumber, int pageSize);
    Task<AnnouncementsShortAndPagesDto> GetBoughtByUserId(Guid userId, int pageNumber, int pageSize);
    Task<int> GetTotalAnnouncements();
    Task<decimal> GetTotalIncome();
    Task<GeneralTopDealDto?> GetTopDeal();
    Task<List<GeneralTopRealtorsDto>> GetTopRealtors(int top);
    Task<List<GeneralTopPropertyDto>> GetTopPropertyTypes(int top);
    Task<List<GeneralTopClientDto>> GetTopClients(int top);
    Task<Guid?> GetPropertyIdByAnnouncementIdAsync(Guid announcementId);
    Task<Guid?> GetStatementIdByAnnouncementIdAsync(Guid announcementId);
    Task<AnnouncementShortDto?> GetAnnouncementShortByOfferId(Guid offerId, Guid userId);
}