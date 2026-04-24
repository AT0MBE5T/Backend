using RealEstateAgency.Application.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IViewService
{
    Task<Guid> InsertAsync(ViewDto view);
}