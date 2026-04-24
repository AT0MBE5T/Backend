using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using ApplicationMapper = RealEstateAgency.Application.Mappers.ApplicationMapper;

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