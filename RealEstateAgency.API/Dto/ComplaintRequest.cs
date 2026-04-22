namespace RealEstateAgency.API.Dto;

public class ComplaintRequest
{
    public required Guid AnnouncementId { get; set; }
    public required string UserNote { get; set; }
    public required Guid TypeId { get; set; }
}