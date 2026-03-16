namespace RealEstateAgency.Application.Dto;

public class PaymentDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public Guid AnnouncementId { get; set; }
}