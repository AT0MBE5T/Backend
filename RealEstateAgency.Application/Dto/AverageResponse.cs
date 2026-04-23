namespace RealEstateAgency.Application.Dto;

public record AverageResponse<T>(T Value, string Error);