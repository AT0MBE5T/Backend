namespace RealEstateAgency.Application.Dtos;

public class PropertyTypeTopRealtorDto
{
    public required string TopRealtorName { get; set; }
    public required int TopRealtorDeals { get; set; }
    public decimal TopRealtorIncome { get; set; }
}