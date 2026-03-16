namespace RealEstateAgency.API.Dto;

public class GeneralStatsResponseDto
{
    public int TotalClosedAnnouncements { get; set; }
    public int TotalIncome { get; set; }
    public string TopDealName { get; set; } = string.Empty;
    public string TopDealStatementType { get; set; } = string.Empty;
    public decimal TopDealPrice { get; set; }
    public string TopDealRealtorName { get; set; } = string.Empty;
    public string TopDealCustomerName { get; set; } = string.Empty;
    public string TopDealSoldDate { get; set; } = string.Empty;
    public string TopRealtorNameFirst { get; set; } = string.Empty;
    public int TopRealtorDealsFirst { get; set; }
    public int TopRealtorIncomeFirst { get; set; }
    public string TopRealtorNameSecond { get; set; } = string.Empty;
    public int TopRealtorDealsSecond { get; set; }
    public int TopRealtorIncomeSecond { get; set; }
    public string TopPropertyTypeNameFirst { get; set; } = string.Empty;
    public int TopPropertyTypeCntFirst { get; set; }
    public decimal TopPropertyTypeAvgPriceFirst { get; set; }
    public string TopPropertyTypeNameSecond { get; set; } = string.Empty;
    public int TopPropertyTypeCntSecond { get; set; }
    public decimal TopPropertyTypeAvgPriceSecond { get; set; }
    public string TopClientNameFirst { get; set; } = string.Empty;
    public int TopClientDealsFirst { get; set; }
    public int TopClientSpentFirst { get; set; }
    public string TopClientNameSecond { get; set; } = string.Empty;
    public int TopClientDealsSecond { get; set; }
    public int TopClientSpentSecond { get; set; }
}