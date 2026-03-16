namespace RealEstateAgency.Application.Dto;

public class PropertyTypeTopClientDto
{
    public required string TopClientName { get; set; }
    public required int TopClientDeals { get; set; }
    public decimal TopClientSpent { get; set; }
}