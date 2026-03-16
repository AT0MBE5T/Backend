namespace RealEstateAgency.API.Dto;

public class LoginResponseDto
{
    public Guid Id { get; set; }
    public required string Login { get; set; }
    public string AccessToken { get; set; } = string.Empty;
}