namespace RealEstateAgency.Core.Dtos;

public class AnnouncementFullDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string StatementTypeName { get; set; }
    public required string PropertyTypeName { get; set; }
    public required decimal Price { get; set; }
    public required string Location { get; set; }
    public required double Area { get; set; }
    public required List<PhotoDto> Photos { get; set; }
    public required string Content { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required int Floors { get; set; }
    public required int Rooms { get; set; }
    public required string Description { get; set; }
    public Guid AuthorId { get; set; }
    public required string Author { get; set; }
    public bool IsVerified { get; set; }
    public bool IsFavorite { get; set; }
    public required int ViewsCnt { get; set; }
    public required DateTime? ClosedAt { get; set; }
}