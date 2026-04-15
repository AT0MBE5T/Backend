using RealEstateAgency.Application.Utils;

namespace RealEstateAgency.API.Dto;

public class ComplaintAdminRequest
{
    public required Guid ComplaintId { get; set; }
    public required Guid AdminId { get; set; }
    public required string AdminNote { get; set; }
    public required Guid StatusId { get; set; }
}