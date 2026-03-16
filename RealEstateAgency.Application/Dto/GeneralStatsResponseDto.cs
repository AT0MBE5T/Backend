namespace RealEstateAgency.Application.Dto;

public class GeneralStatsResponseDto
{
    public int TotalClosedAnnouncements { get; set; }
    public decimal TotalIncome { get; set; }
    public string TopDealName { get; set; } = string.Empty;
    public string TopDealStatementType { get; set; } = string.Empty;
    public decimal TopDealPrice { get; set; }
    public string TopDealRealtorName { get; set; } = string.Empty;
    public string TopDealCustomerName { get; set; } = string.Empty;
    public string TopDealSoldDate { get; set; } = string.Empty;
    public string TopRealtorNameFirst { get; set; } = string.Empty;
    public int TopRealtorDealsFirst { get; set; }
    public decimal TopRealtorIncomeFirst { get; set; }
    public string TopRealtorNameSecond { get; set; } = string.Empty;
    public int TopRealtorDealsSecond { get; set; }
    public decimal TopRealtorIncomeSecond { get; set; }
    public string TopPropertyTypeNameFirst { get; set; } = string.Empty;
    public int TopPropertyTypeCntFirst { get; set; }
    public decimal TopPropertyTypeAvgPriceFirst { get; set; }
    public string TopPropertyTypeNameSecond { get; set; } = string.Empty;
    public int TopPropertyTypeCntSecond { get; set; }
    public decimal TopPropertyTypeAvgPriceSecond { get; set; }
    public string TopClientNameFirst { get; set; } = string.Empty;
    public int TopClientDealsFirst { get; set; }
    public decimal TopClientSpentFirst { get; set; }
    public string TopClientNameSecond { get; set; } = string.Empty;
    public int TopClientDealsSecond { get; set; }
    public decimal TopClientSpentSecond { get; set; }
}