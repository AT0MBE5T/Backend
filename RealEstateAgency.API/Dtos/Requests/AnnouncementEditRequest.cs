namespace RealEstateAgency.API.Dtos.Requests;

public class AnnouncementEditRequest
{
    public Guid AnnouncementId { get; set; }
    public List<IFormFile> NewPhotos { get; set; } = [];
    public List<Guid> DeletedImageIds { get; set; } = [];
    public List<string> ExistingImageOrder { get; set; } = [];
    public Guid PropertyTypeId { get; set; }
    public Guid StatementTypeId { get; set; }
    public string Location { get; set; } = string.Empty;
    public double Area { get; set; }
    public int Floors { get; set; }
    public int Rooms { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime? UpdatedAt { get; set; }
}