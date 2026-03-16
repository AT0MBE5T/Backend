namespace RealEstateAgency.Core.Models;

public class PropertyTypeTopDealCoreDto
{
    public required string TopDealName { get; set; }
    public required string TopDealStatementType { get; set; }
    public decimal TopDealPrice { get; set; }
    public required string TopDealRealtorName { get; set; }
    public required string TopDealCustomerName { get; set; }
    public DateTime TopDealSoldDate { get; set; }
}