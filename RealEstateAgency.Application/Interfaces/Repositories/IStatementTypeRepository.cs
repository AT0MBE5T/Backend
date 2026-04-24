using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IStatementTypeRepository
{
    Task<List<StatementType>> GetAllAsync();
}