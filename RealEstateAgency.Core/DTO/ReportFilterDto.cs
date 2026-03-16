namespace RealEstateAgency.Core.DTO;

public class ReportFilterDto
{
    public string DateFrom { get; set; }
    public string DateTo { get; set; }
    public string ClientName { get; set; }
    public Guid PropertyTypeId { get; set; }
}