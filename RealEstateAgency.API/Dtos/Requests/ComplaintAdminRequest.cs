namespace RealEstateAgency.API.Dtos.Requests;

public class ComplaintAdminRequest
{
    public required Guid ComplaintId { get; set; }
    public required Guid AdminId { get; set; }
    public required string AdminNote { get; set; }
    public required Guid StatusId { get; set; }
}