namespace RealEstateAgency.Core.Dtos;

public class ComplaintGridDto
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public required Guid AnnouncementId { get; set; }
    public required string AnnouncementName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserNote { get; set; } = string.Empty;
    public string? AdminName { get; set; }
    public string? AdminNote { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public required string TypeName { get; set; }
    public string StatusName { get; set; } = string.Empty;
}