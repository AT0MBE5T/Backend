namespace RealEstateAgency.Application.Dtos;

public record MainStatsDataRawDto(int TotalDeals, int TotalActive, decimal SumPrice, double SumArea, decimal Revenue, decimal AvgPrice, int Count);