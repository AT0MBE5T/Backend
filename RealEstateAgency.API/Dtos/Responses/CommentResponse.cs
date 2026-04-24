namespace RealEstateAgency.API.Dtos.Responses;

public class CommentResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Text { get; set; }
    public required string Author { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required Guid AnnouncementId { get; set; }
}