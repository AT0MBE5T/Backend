using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IHubService
{
    Task NotifyNewOfferAsync(AnnouncementShortDto? offerDto);
    Task NotifyUpdateOfferAsync(AnnouncementShortDto? offerDto);
    Task DeleteOfferAsync(Guid announcementId);
}