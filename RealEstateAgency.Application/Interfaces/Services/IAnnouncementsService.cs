using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IAnnouncementsService
{
    Task<AnnouncementGetEditRequest?> GetAnnouncementForEditByIdAsync(Guid announcementId);
    Task<Guid> GetAuthorOfferIdByQuestionId(Guid questionId);
    Task<string> SwitchVerifyAnnouncement(SwitchVerificationCommandDto commandDto);
    Task<AddAnnouncementResponseDto> CreateAnnouncementAsync(CreateAnnouncementCommandDto commandDto);
    Task<string> UpdateAnnouncementAsync(AnnouncementUpdateCommandDto commandDto);
    Task<string> DeleteAnnouncementAsync(DeleteAnnouncementCommandDto commandDto);
    Task<AnnouncementsShortAndPagesDto> GetSearchDataPaginated(string text, List<string> filters, int sortId, int page, int limit, Guid userId);
    Task<AnnouncementFullDto?> GetAnnouncementFullById(AnnouncementInfoCommandDto commandDto);
    Task<AnnouncementsShortAndPagesDto> GetBoughtAnnouncementsByUserId(Guid userId, int page, int limit);
    Task<AnnouncementsShortAndPagesDto> GetSoldAnnouncementsByUserId(Guid userId, int page, int limit);
    Task<AnnouncementsShortAndPagesDto> GetPlacedAnnouncementsByUserId(Guid userId, int page, int limit);
    Task<List<AnnouncementGridDto>> GetAnnouncementsGrid();
    Task<CloseAnnouncementResponseDto> CloseAnnouncement(CloseAnnouncementCommandDto commandDto);
}