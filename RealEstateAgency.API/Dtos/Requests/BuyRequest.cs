namespace RealEstateAgency.API.Dtos.Requests;

public class BuyRequest
{
    public Guid CustomerId { get; set; }
    public Guid AnnouncementId { get; set; }
}