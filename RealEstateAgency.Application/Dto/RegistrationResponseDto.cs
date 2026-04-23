using Microsoft.AspNetCore.Http;

namespace RealEstateAgency.Application.Dto;

public record RegistrationResponseDto(
    Guid UserId, 
    string AccessToken, 
    string RefreshToken,
    List<string> Errors,
    int StatusCode
);