using Microsoft.AspNetCore.SignalR;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Infrastructure.Hubs;

namespace RealEstateAgency.Infrastructure.Services;

public class HubService(IHubContext<MessageHub> hubContext) : IHubService
{
    public async Task NotifyNewOfferAsync(AnnouncementShort? offerDto)
    {
        await hubContext.Clients.Group("offers_global")
            .SendAsync("ReceiveOffer", offerDto);
    }
    
    public async Task NotifyUpdateOfferAsync(AnnouncementShort? offerDto)
    {
        await hubContext.Clients.Group("offers_global")
            .SendAsync("UpdateOffer", offerDto);
    }
    
    public async Task DeleteOfferAsync(Guid announcementId)
    {
        await hubContext.Clients.Group("offers_global")
            .SendAsync("DeleteOffer", announcementId);
    }
}