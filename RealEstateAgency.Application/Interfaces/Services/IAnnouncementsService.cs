using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IAnnouncementsService
{
    Task<AnnouncementGetEditRequest?> GetAnnouncementForEditByIdAsync(Guid announcementId);
    Task<bool> SwitchVerificateAnnouncement(Guid announcementId, Guid userId);
    Task<Guid?> AddAnnouncement(Guid userId, AnnouncementDto announcementDto);
    Task<bool> UpdateAnnouncementAsync(Guid announcementId, Guid userId, AnnouncementDto announcementDto);
    Task<bool> DeleteAnnouncementAsync(Guid announcementId, Guid userId);
    Task<AnnouncementsShortAndPages> GetSearchDataPaginated(string text, List<string> filters, int sortId, int page, int limit, Guid? userId);
    Task<AnnouncementFull?> GetAnnouncementFullById(Guid id, Guid? userId);
    Task<int> GetPages();
    Task<bool> SetClosedAt(Guid id);
    Task<AnnouncementsShortAndPages> GetBoughtAnnouncementsByUserId(Guid userId, int page);
    Task<AnnouncementsShortAndPages> GetSoldAnnouncementsByUserId(Guid userId, int page);
    Task<AnnouncementsShortAndPages> GetPlacedAnnouncementsByUserId(Guid userId, int page);
    Task<byte[]> GetBytesByAnnouncementIdAsync(Guid announcementId);
    Task<Guid> GetImageIdByAnnouncementIdAsync(Guid announcementId);
    Task<Guid?> GetPropertyIdByAnnouncementIdAsync(Guid announcementId);
    Task<Guid?> GetStatementIdByAnnouncementIdAsync(Guid announcementId);
    Task<AnnouncementShort?> GetAnnouncementShortByOfferId(Guid offerId, Guid userId);
}