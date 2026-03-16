using RealEstateAgency.Application.Dto;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IStatementTypeService
{
    Task<List<StatementTypeDto>> GetAll();
}