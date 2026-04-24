namespace RealEstateAgency.API.Dtos.Responses;

public class LoginResponse
{
    public Guid Id { get; set; }
    public required string Login { get; set; }
    public string AccessToken { get; set; } = string.Empty;
}