namespace RealEstateAgency.Core.Dtos;

public class GeneralTopClientDto
{
    public string TopClientName { get; set; } = string.Empty;
    public int TopClientDeals { get; set; }
    public decimal TopClientSpent { get; set; }
}