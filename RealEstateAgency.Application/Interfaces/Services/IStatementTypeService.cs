using RealEstateAgency.Application.Dtos;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IStatementTypeService
{
    Task<List<StatementTypeDto>> GetAll();
}