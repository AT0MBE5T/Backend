namespace RealEstateAgency.Application.Dto;

public class AnnouncementDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StatementId { get; set; }
    public bool IsActive { get; set; }
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; set; } = null;
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
}