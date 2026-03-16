namespace RealEstateAgency.Application.Dto;

public class CommentDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Text { get; set; }
    public required Guid UserId { get; set; }
    public required Guid AnnouncementId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}