namespace RealEstateAgency.Core.Dtos;

public class AnnouncementShortDto
{
    public Guid Id { get; set; }
    public required string PhotoUrl { get; set; }
    public required string Title { get; set; }
    public required string StatementTypeName { get; set; }
    public required string PropertyTypeName { get; set; }
    public decimal Price { get; set; }
    public required string Location { get; set; }
    public double Area { get; set; }
    public required bool IsVerified { get; set; }
    public bool IsFavorite { get; set; }
    public required int ViewsCnt { get; set; }
    public required DateTime? ClosedAt { get; set; }
    public required DateTime PublishedAt { get; set; }
}