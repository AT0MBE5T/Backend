namespace RealEstateAgency.Core.DTO;

public class GeneralTopDeal
{
    public string TopDealName { get; set; } = string.Empty;
    public string TopDealStatementType { get; set; } = string.Empty;
    public decimal TopDealPrice { get; set; }
    public string TopDealRealtorName { get; set; } = string.Empty;
    public string TopDealCustomerName { get; set; } = string.Empty;
    public DateTime TopDealSoldDate { get; set; }
}