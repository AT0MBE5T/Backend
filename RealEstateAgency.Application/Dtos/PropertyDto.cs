namespace RealEstateAgency.Application.Dtos;

public class PropertyDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PropertyTypeId { get; set; }
    public string Location { get; set; } = string.Empty;
    public double Area { get; set; }
    public int Floors { get; set; }
    public int Rooms { get; set; }
    public string Description { get; set; } = string.Empty;
}