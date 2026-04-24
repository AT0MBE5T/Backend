using RealEstateAgency.Application.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IPaymentService
{
    Task<Guid?> InsertPayment(PaymentDto paymentDto);
    Task<bool> IsExistByAnnouncementIdAsync(Guid announcementId);
}