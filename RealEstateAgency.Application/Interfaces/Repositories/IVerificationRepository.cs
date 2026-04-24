using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IVerificationRepository
{
    Task<Guid> Insert(Verification verification);
    Task<bool> Delete(Verification verification);
}