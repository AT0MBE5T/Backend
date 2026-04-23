using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IAnnouncementsService
{
    Task<AnnouncementGetEditRequest?> GetAnnouncementForEditByIdAsync(Guid announcementId);
    Task<Guid> GetAuthorOfferIdByQuestionId(Guid questionId);
    Task<string> SwitchVerifyAnnouncement(SwitchVerificationCommand command);
    Task<AddAnnouncementResponse> CreateAnnouncementAsync(CreateAnnouncementCommand command);
    Task<string> UpdateAnnouncementAsync(AnnouncementUpdateCommand command);
    Task<string> DeleteAnnouncementAsync(DeleteAnnouncementCommand command);
    Task<AnnouncementsShortAndPages> GetSearchDataPaginated(string text, List<string> filters, int sortId, int page, int limit, Guid userId);
    Task<AnnouncementFull?> GetAnnouncementFullById(AnnouncementInfoCommand command);
    Task<bool> SetClosedAt(Guid id);
    Task<AnnouncementsShortAndPages> GetBoughtAnnouncementsByUserId(Guid userId, int page, int limit);
    Task<AnnouncementsShortAndPages> GetSoldAnnouncementsByUserId(Guid userId, int page, int limit);
    Task<AnnouncementsShortAndPages> GetPlacedAnnouncementsByUserId(Guid userId, int page, int limit);
    Task<byte[]> GetBytesByAnnouncementIdAsync(Guid announcementId);
    Task<Guid> GetImageIdByAnnouncementIdAsync(Guid announcementId);
    Task<Guid?> GetPropertyIdByAnnouncementIdAsync(Guid announcementId);
    Task<Guid?> GetStatementIdByAnnouncementIdAsync(Guid announcementId);
    Task<AnnouncementShort?> GetAnnouncementShortByOfferId(Guid offerId, Guid userId);
    Task<List<AnnouncementGrid>> GetAnnouncementsGrid();
    Task<CloseAnnouncementResponse> CloseAnnouncement(CloseAnnouncementCommand command);
}