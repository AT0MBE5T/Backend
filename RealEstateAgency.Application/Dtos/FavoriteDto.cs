namespace RealEstateAgency.Application.Dtos;

public class FavoriteDto
{
    public required Guid UserId { get; set; }
    public required Guid AnnouncementId { get; set; }
    public DateTime CreatedAt { get; set; }
}