namespace RealEstateAgency.Application.Dto;

public class ReportUserDto
{
    public required string Login { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}