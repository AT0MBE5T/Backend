using Microsoft.AspNetCore.SignalR;
using RealEstateAgency.Application.Interfaces;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Infrastructure.Hubs;

namespace RealEstateAgency.Infrastructure.Services;

public class HubService(IHubContext<MessageHub, IChatClient> hubContext) : IHubService
{
    public async Task NotifyNewOfferAsync(AnnouncementShortDto offerDto)
    {
        await hubContext.Clients.Group("offers_global")
            .ReceiveOffer(offerDto);
    }
    
    public async Task NotifyNewOfferWpfAsync(AnnouncementGridDto model)
    {
        await hubContext.Clients.Group("offers_global")
            .ReceiveOfferWPF(model);
    }
    
    public async Task NotifyNewSupportAsync(SupportGridDto support)
    {
        await hubContext.Clients.Group("supports_global")
            .ReceiveSupportWPF(support);
    }
    
    public async Task NotifyUpdateSupportAsync(SupportGridDto support)
    {
        await hubContext.Clients.Group("supports_global")
            .UpdateSupportWPF(support);
    }
    
    public async Task NotifyNewComplaintAsync(ComplaintGridDto complaint)
    {
        await hubContext.Clients.Group("complaints_global")
            .ReceiveComplaintWPF(complaint);
    }
    
    public async Task NotifyUpdateComplaintAsync(ComplaintGridDto complaint)
    {
        await hubContext.Clients.Group("complaints_global")
            .UpdateComplaintWPF(complaint);
    }
    
    public async Task NotifyUpdateOfferAsync(AnnouncementShortDto offerDto)
    {
        await hubContext.Clients.Group("offers_global").UpdateOffer(offerDto);
    }
    
    public async Task NotifyUpdateOfferWpfAsync(AnnouncementGridDto model)
    {
        await hubContext.Clients.Group("offers_global")
            .UpdateOfferWPF(model);
    }
    
    public async Task NotifyUpdateFullOfferAsync(Guid changedById, AnnouncementFullDto offerDto)
    {
        await hubContext.Clients.Group(offerDto.Id.ToString()).UpdateFullOffer(changedById.ToString(), offerDto);
    }
    
    public async Task DeleteOfferAsync(Guid announcementId)
    {
        await hubContext.Clients.Group("offers_global").DeleteOffer(announcementId);
        await hubContext.Clients.Group(announcementId.ToString()).DeleteFullOffer();
        await hubContext.Clients.Group(announcementId.ToString()).DeleteOfferFullComment();
        await hubContext.Clients.Group(announcementId.ToString()).DeleteOfferFullQuestion();
        
        await hubContext.Clients.Group("offers_global")
            .DeleteOfferWPF(announcementId);
    }
}