namespace RealEstateAgency.API.Dto;

public class PropertyTypeStatsResponse
{
    public int TotalPlacedAnnouncements { get; set; }
    public int TotalDeals { get; set; }
    public decimal TotalIncome { get; set; }
    public required string TopDealName { get; set; }
    public required string TopDealStatementType { get; set; }
    public decimal TopDealPrice { get; set; }
    public required string TopDealRealtorName { get; set; }
    public required string TopDealCustomerName { get; set; }
    public required string TopDealSoldDate { get; set; }
    public required string TopRealtorName { get; set; }
    public int TopRealtorDeals { get; set; }
    public decimal TopRealtorIncome { get; set; }
    public required string TopClientName { get; set; }
    public int TopClientDeals { get; set; }
    public int TopClientSpent { get; set; }
}