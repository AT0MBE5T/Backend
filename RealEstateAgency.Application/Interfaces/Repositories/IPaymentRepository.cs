using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<Guid> Insert(Payment payment);
}