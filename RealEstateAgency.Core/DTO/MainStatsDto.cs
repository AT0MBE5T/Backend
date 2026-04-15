namespace RealEstateAgency.Core.DTO;

public class MainStatsDto
{
    public int TotalActive { get; set; }
    public decimal AvgPricePerMeter { get; set; }
    public int TotalDeals { get; set; }
    public decimal Revenue { get; set; }
}