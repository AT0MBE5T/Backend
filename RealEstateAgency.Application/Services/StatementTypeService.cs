using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;

namespace RealEstateAgency.Application.Services;

public class StatementTypeService(IStatementTypeRepository repository, ApplicationMapper mapper) : IStatementTypeService
{
    public async Task<List<StatementTypeDto>> GetAll()
    {
        var statements = await repository.GetAllAsync();
        
        var res = statements
            .Select(mapper.StatementTypeEntityToStatementTypeDto)
            .ToList();

        return res;
    }
}