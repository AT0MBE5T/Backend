namespace RealEstateAgency.Application.Dto;

public class PropertyTypeStatsDto
{
    public int TotalPlacedAnnouncements { get; set; }
    public int TotalDeals { get; set; }
    public decimal TotalIncome { get; set; }
    public string TopDealName { get; set; } = string.Empty;
    public string TopDealStatementType { get; set; } = string.Empty;
    public decimal TopDealPrice { get; set; }
    public string TopDealRealtorName { get; set; } = string.Empty;
    public string TopDealCustomerName { get; set; } = string.Empty;
    public string TopDealSoldDate { get; set; } = string.Empty;
    public string TopRealtorName { get; set; } = string.Empty;
    public int TopRealtorDeals { get; set; }
    public decimal TopRealtorIncome { get; set; }
    public string TopClientName { get; set; } = string.Empty;
    public int TopClientDeals { get; set; }
    public decimal TopClientSpent { get; set; }
}