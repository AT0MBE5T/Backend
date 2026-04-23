using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IHubService
{
    Task NotifyNewOfferAsync(AnnouncementShort? offerDto);
    Task NotifyUpdateOfferAsync(AnnouncementShort? offerDto);
    Task DeleteOfferAsync(Guid announcementId);
}