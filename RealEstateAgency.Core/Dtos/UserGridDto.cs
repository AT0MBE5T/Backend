namespace RealEstateAgency.Core.Dtos;

public class UserGridDto
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
    public Guid RoleId { get; set; }
    public required string RoleName { get; set; }
    public required int TotalOffersPlacedCnt { get; set; }
    public required int OffersClosedCnt { get; set; }
    public required int OffersBoughtCnt { get; set; }
    public required decimal TotalRevenue { get; set; }
    public required decimal TotalSpent { get; set; }
}