using Microsoft.AspNetCore.Mvc;

namespace RealEstateAgency.API.Dto;

public class AnnouncementResponse
{
    public Guid Id { get; set; }
    public required string PhotoUrl { get; set; }
    public required string Title { get; set; }
    public required string StatementTypeName { get; set; }
    public required string PropertyTypeName { get; set; }
    public decimal Price { get; set; }
    public required string Location { get; set; }
    public double Area { get; set; }
    public bool IsVerified { get; set; }
    public bool IsFavorite { get; set; }
    public int ViewsCnt { get; set; }
}