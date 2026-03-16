namespace RealEstateAgency.API.Dto;

public class BuyRequest
{
    public Guid CustomerId { get; set; }
    public Guid AnnouncementId { get; set; }
}