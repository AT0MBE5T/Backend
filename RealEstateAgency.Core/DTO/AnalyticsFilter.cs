namespace RealEstateAgency.Core.DTO;

public class AnalyticsFilter
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public Guid? PropertyTypeId { get; set; }
    public Guid? StatementTypeId { get; set; }
    public bool IsBought { get; set; }
}