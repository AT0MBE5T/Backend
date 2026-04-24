using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Interfaces.Services;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatementTypesController(IStatementTypeService statementTypeService): ControllerBase
{
    [HttpGet("get-statement-types")]
    public async Task<IActionResult> GetStatementTypes()
    {
        var statementTypesList = await statementTypeService.GetAll();
        return Ok(statementTypesList);
    }
}