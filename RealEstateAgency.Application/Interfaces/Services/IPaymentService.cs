using RealEstateAgency.Application.Dto;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IPaymentService
{
    Task<Guid?> InsertPayment(PaymentDto paymentDto);
}