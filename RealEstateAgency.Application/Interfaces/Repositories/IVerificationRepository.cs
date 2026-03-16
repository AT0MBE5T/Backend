using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IVerificationRepository
{
    Task<Guid> Insert(Verification verification);
    Task<bool> Delete(Verification verification);
}