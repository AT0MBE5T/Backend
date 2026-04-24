namespace RealEstateAgency.Application.Dtos;

public class UserDto
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Login { get; set; }
    public required int Age { get; set; }
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public required string AvatarUrl { get; set; }
    public required string PublicId { get; set; }
    public List<string> Roles { get; set; } = [];
}