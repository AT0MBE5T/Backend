namespace RealEstateAgency.Core.DTO;

public class MessageCreatedEvent
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
}