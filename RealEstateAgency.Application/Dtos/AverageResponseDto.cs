namespace RealEstateAgency.Application.Dtos;

public record AverageResponseDto<T>(T Value, string Error);