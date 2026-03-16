namespace RealEstateAgency.API.Dto;

public class ReportUserRequest
{
    public required string Login { get; set; }
    public required string DateFrom { get; set; }
    public string DateTo { get; set; } = string.Empty;
}