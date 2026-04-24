namespace RealEstateAgency.Core.Dtos;

public class AuditGridDto
{
    public Guid Id { get; set; }
    public Guid ActionId { get; set; }
    public required string ActionName { get; set; }
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string Details { get; set; }
}