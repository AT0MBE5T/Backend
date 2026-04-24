namespace RealEstateAgency.Core.Dtos;

public class AnnouncementsShortAndPagesDto
{
    public List<AnnouncementShortDto> Data { get; set; } = [];
    public int PagesCnt { get; set; }
    public int TotalItems { get; set; }
}