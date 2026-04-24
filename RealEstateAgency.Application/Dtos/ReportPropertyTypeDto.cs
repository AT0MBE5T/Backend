namespace RealEstateAgency.Application.Dtos;

public class ReportPropertyTypeDto
{
    public Guid PropertyTypeId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}