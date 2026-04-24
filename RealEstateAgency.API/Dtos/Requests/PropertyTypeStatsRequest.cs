namespace RealEstateAgency.API.Dtos.Requests;

public class PropertyTypeStatsRequest
{
    public Guid PropertyTypeId { get; set; }
    public required string DateFrom { get; set; }
    public string DateTo { get; set; } = string.Empty;
}