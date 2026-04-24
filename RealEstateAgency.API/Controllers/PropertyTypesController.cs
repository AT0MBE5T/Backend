using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Interfaces.Services;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyTypesController(IPropertyTypeService propertyTypeService): ControllerBase
{
    [HttpGet("get-property-types")]
    public async Task<IActionResult> GetPropertyTypes()
    {
        var propertyTypesList = await propertyTypeService.GetAll();
        return Ok(propertyTypesList);
    }
}