namespace RealEstateAgency.Core.Dtos;

public class CommentGridDto
{
    public Guid Id { get; set; }
    public required Guid AnnouncementId { get; set; }
    public required string Text { get; set; }
    public required string Author { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required string StatementTitle { get; set; }
}