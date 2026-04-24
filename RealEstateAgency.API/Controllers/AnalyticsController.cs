using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(IAnalyticService service): ControllerBase
{
    [HttpGet("get-main-stats")]
    public async Task<IActionResult> GetMainStats([FromQuery] AnalyticsFilterDto filterDto)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var result = await service.GetMainStats(filterDto);
        return Ok(result);
    }
    
    [HttpGet("get-distribution-property-type-data")]
    public async Task<IActionResult> GetDistributionPropertyTypeData([FromQuery] AnalyticsFilterDto filterDto)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var result = await service.GetDistributionPropertyTypeData(filterDto);
        return Ok(result);
    }
    
    [HttpGet("get-distribution-statement-type-data")]
    public async Task<IActionResult> GetDistributionStatementTypeData([FromQuery] AnalyticsFilterDto filterDto)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var result = await service.GetDistributionStatementTypeData(filterDto);
        return Ok(result);
    }
    
    [HttpGet("get-market-trends")]
    public async Task<IActionResult> GetMarketTrends([FromQuery] AnalyticsFilterDto filterDto)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var result = await service.GetMarketTrends(filterDto);
        return Ok(result);
    }
    
    [HttpGet("get-filtered-announcements")]
    public async Task<IActionResult> GetFilteredAnnouncements([FromQuery] AnalyticsFilterDto filterDto)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var result = await service.GetFilteredAnnouncements(filterDto);
        return Ok(result);
    }
    
    [HttpGet("get-realtors")]
    public async Task<IActionResult> GetRealtors([FromQuery] AnalyticsFilterDto filterDto)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var result = await service.GetRealtors(filterDto);
        return Ok(result);
    }
}