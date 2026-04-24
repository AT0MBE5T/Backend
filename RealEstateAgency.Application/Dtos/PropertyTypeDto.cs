namespace RealEstateAgency.Application.Dtos;

public class PropertyTypeDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
}