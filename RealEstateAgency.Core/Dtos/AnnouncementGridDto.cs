namespace RealEstateAgency.Core.Dtos;

public class AnnouncementGridDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public Guid StatementTypeId { get; set; }
    public Guid PropertyTypeId { get; set; }
    public required string StatementTypeName { get; set; }
    public required string PropertyTypeName { get; set; }
    public decimal Price { get; set; }
    public bool IsVerified { get; set; }
    public int ViewsCnt { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public Guid AuthorId { get; set; }
    public required string Author { get; set; }
}