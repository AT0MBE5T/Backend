namespace RealEstateAgency.Application.Dtos;

public record RegistrationResponseDto(
    Guid UserId, 
    string AccessToken, 
    string RefreshToken,
    List<string> Errors,
    int StatusCode
);