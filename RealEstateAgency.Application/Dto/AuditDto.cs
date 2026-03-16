namespace RealEstateAgency.Application.Dto;

public class AuditDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required Guid ActionId { get; set; }
    public required Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required string Details { get; set; }
}