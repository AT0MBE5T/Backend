namespace RealEstateAgency.Application.Dtos;

public class ImageDto
{
    public Guid? PropertyId { get; set; }
    public required string PhotoUrl { get; set; }
    public required string PublicId { get; set; }
    public required int OrderIndex { get; set; }
}