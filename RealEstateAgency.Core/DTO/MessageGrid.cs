namespace RealEstateAgency.Core.DTO;

public class MessageGrid
{
    public Guid Id { get; set; }
    public required string Text { get; set; }
    public Guid UserFromId { get; set; }
    public required string UserFromLogin { get; set; }
    public Guid UserToId { get; set; }
    public required string UserToLogin { get; set; }
    public DateTime CreatedAt { get; set; }
}