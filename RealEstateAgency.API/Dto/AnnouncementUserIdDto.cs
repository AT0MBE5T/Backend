namespace RealEstateAgency.API.Dto;

public class AnnouncementUserIdDto
{
    public Guid AnnouncementId { get; set; }
    public Guid? UserId { get; set; }
}