using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<Guid> Insert(Payment payment);
    Task<bool> IsExistByAnnouncementId(Guid announcementId);
}