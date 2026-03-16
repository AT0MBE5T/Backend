namespace RealEstateAgency.Application.Dto;

public class ComplaintDto
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required Guid AnnouncementId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserNote { get; set; } = string.Empty;
    public Guid? AdminId { get; set; }
    public string? AdminNote { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public required Guid TypeId { get; set; }
    public required Guid StatusId { get; set; }
}