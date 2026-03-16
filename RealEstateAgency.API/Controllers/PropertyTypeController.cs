using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dto;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Services;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyTypeController(IPropertyTypeService propertyTypeService): ControllerBase
{
    [HttpGet("get-property-types")]
    public async Task<IActionResult> GetPropertyTypes()
    {
        var propertyTypesList = await propertyTypeService.GetAll();
        return Ok(propertyTypesList);
    }
}