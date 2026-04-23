using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dto;
using RealEstateAgency.API.Mapper;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;

namespace RealEstateAgency.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportController(
    IReportsService reportService,
    ApiMapper mapper) : ControllerBase
{
    [HttpGet("get-general-report")]
    public async Task<IActionResult> GetGeneralReport()
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var res = await reportService.GetGeneralReport();
        return Ok(res);
    }
    
    [HttpPost("get-report-by-property-type-id")]
    public async Task<IActionResult> GetReportByPropertyTypeId([FromBody] PropertyTypeStatsRequest request)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var mapped = new ReportPropertyTypeDto
        {
            PropertyTypeId = request.PropertyTypeId,
            DateFrom = DateTime.Parse(request.DateFrom),
            DateTo = string.IsNullOrEmpty(request.DateTo)
                ? default
                : DateTime.Parse(request.DateTo)
        };
        
        var result = await reportService.GetReportByPropertyTypeId(mapped);
        return result is not null
            ? Ok(result)
            : NotFound();
    }
    
    [HttpPost("get-report-by-user-login")]
    public async Task<IActionResult> GetReportByUserLogin([FromBody] ReportUserRequest request)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var mapped = mapper.ReportUserRequestToReportUserDto(request);

        var result = await reportService.GetReportByUserLogin(mapped);
        return result is not null
            ? Ok(result)
            : NotFound();
    }
    
    [HttpGet("get-report-by-user-id/{userId:guid}")]
    public async Task<IActionResult> GetReportByUserId(Guid userId)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();   
        
        var result = await reportService.GetReportByUserId(userId);
        return result is not null
            ? Ok(result)
            : NotFound();
    }
}