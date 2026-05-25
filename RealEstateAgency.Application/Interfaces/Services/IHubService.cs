using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IHubService
{
    Task NotifyNewOfferAsync(AnnouncementShortDto offerDto);
    Task NotifyUpdateOfferAsync(AnnouncementShortDto offerDto);
    Task NotifyNewOfferWpfAsync(AnnouncementGridDto model);
    Task NotifyUpdateOfferWpfAsync(AnnouncementGridDto model);
    Task NotifyUpdateFullOfferAsync(Guid changedById, AnnouncementFullDto offerDto);
    Task DeleteOfferAsync(Guid announcementId);
    Task NotifyUpdateComplaintAsync(ComplaintGridDto complaint);
    Task NotifyNewComplaintAsync(ComplaintGridDto complaint);
    Task NotifyUpdateSupportAsync(SupportGridDto support);
    Task NotifyNewSupportAsync(SupportGridDto support);
}