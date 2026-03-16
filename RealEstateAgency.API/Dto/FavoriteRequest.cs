namespace RealEstateAgency.API.Dto;

public class FavoriteRequest
{
    public Guid UserId { get; set; }
    public Guid AnnouncementId { get; set; }
}