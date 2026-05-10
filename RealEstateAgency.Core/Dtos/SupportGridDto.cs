namespace RealEstateAgency.Core.Dtos;

public class SupportGridDto
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserNote { get; set; } = string.Empty;
    public string? AdminName { get; set; }
    public DateTime? ClosedAt { get; set; }
}