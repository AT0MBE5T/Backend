using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IStatementTypeRepository
{
    Task<List<StatementType>> GetAllAsync();
}