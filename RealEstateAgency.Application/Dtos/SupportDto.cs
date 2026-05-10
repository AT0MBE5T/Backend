namespace RealEstateAgency.Application.Dtos;

public class SupportDto
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserNote { get; set; } = string.Empty;
    public Guid? AdminId { get; set; }
    public DateTime? ClosedAt { get; set; }
}