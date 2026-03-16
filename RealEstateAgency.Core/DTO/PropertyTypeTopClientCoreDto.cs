namespace RealEstateAgency.Core.DTO;

public class PropertyTypeTopClientCoreDto
{
    public required string TopClientName { get; set; }
    public required int TopClientDeals { get; set; }
    public decimal TopClientSpent { get; set; }
}