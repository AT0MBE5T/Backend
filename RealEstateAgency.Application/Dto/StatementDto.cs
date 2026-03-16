namespace RealEstateAgency.Application.Dto;

public class StatementDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required Guid StatementTypeId { get; set; }
    public required Guid PropertyId { get; set; }
    public required Guid UserId { get; set; }
    public required decimal Price { get; set; }
    public required DateTime CreatedAt { get; set; }
}