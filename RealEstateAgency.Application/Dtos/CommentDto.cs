namespace RealEstateAgency.Application.Dtos;

public class CommentDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Text { get; set; }
    public required Guid UserId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public required Guid AnnouncementId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}