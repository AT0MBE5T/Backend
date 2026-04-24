namespace RealEstateAgency.Core.Dtos;

public class GeneralTopPropertyDto
{
    public string TopPropertyTypeName { get; set; } = string.Empty;
    public int TopPropertyTypeCnt { get; set; }
    public decimal TopPropertyTypeAvgPrice { get; set; }
}