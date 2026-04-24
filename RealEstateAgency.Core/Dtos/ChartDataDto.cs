namespace RealEstateAgency.Core.Dtos;

public class ChartDataDto
{
    public required string Label { get; set; }
    public int Value { get; set; }
    public Guid Id { get; set; }
}