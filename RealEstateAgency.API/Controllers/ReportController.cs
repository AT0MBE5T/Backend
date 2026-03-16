using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dto;
using RealEstateAgency.API.Mapper;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Services;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController(
    IReportsService reportService,
    IPropertyTypeService propertyTypeService,
    IAccountService accountService,
    ApiMapper mapper) : ControllerBase
{
    [HttpGet("get-general-report")]
    public async Task<IActionResult> GetGeneralReport()
    {
        var res = await reportService.GetGeneralReport();
        return Ok(res);
    }
    
    [HttpPost("get-report-by-property-type-id")]
    public async Task<IActionResult> GetReportByPropertyTypeId([FromBody] PropertyTypeStatsRequest request)
    {
        var mapped = new ReportPropertyTypeDto
        {
            PropertyTypeId = request.PropertyTypeId,
            DateFrom = DateTime.Parse(request.DateFrom),
            DateTo = string.IsNullOrEmpty(request.DateTo)
                ? default
                : DateTime.Parse(request.DateTo)
        };
        
        return mapped.DateTo == default
            ? Ok(await propertyTypeService.GetReportByPropertyTypeDate(mapped))
            : Ok(await propertyTypeService.GetReportByPropertyTypeDateSpan(mapped));
    }
    
    [HttpPost("get-report-by-user-login")]
    public async Task<IActionResult> GetReportByUserLogin([FromBody] ReportUserRequest request)
    {
        var mapped = mapper.ReportUserRequestToReportUserDto(request);

        var res = mapped.DateTo == default
            ? await accountService.GetReportByUserLoginDate(mapped)
            : await accountService.GetReportByUserLoginDateSpan(mapped);
        
        return res == null
            ? NotFound()
            : Ok(res);
    }
}