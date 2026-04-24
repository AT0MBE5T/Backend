namespace RealEstateAgency.Core.Dtos;

public class RealtorGridDto
{
    public Guid Id { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Login { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public int Age { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public DateTime CreatedAt { get; set; }
    public int OffersCnt { get; set; }
    public decimal Revenue { get; set; }
}