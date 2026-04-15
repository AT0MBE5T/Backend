using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Services;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticController(IAnalyticService service): ControllerBase
{
    [HttpGet("get-main-stats")]
    public async Task<IActionResult> GetMainStats([FromQuery] AnalyticsFilter filter)
    {
        var result = await service.GetMainStats(filter);
        return Ok(result);
    }
    
    [HttpGet("get-distribution-property-type-data")]
    public async Task<IActionResult> GetDistributionPropertyTypeData([FromQuery] AnalyticsFilter filter)
    {
        var result = await service.GetDistributionPropertyTypeData(filter);
        return Ok(result);
    }
    
    [HttpGet("get-distribution-statement-type-data")]
    public async Task<IActionResult> GetDistributionStatementTypeData([FromQuery] AnalyticsFilter filter)
    {
        var result = await service.GetDistributionStatementTypeData(filter);
        return Ok(result);
    }
    
    [HttpGet("get-market-trends")]
    public async Task<IActionResult> GetMarketTrends([FromQuery] AnalyticsFilter filter)
    {
        var result = await service.GetMarketTrends(filter);
        return Ok(result);
    }
    
    [HttpGet("get-filtered-announcements")]
    public async Task<IActionResult> GetFilteredAnnouncements([FromQuery] AnalyticsFilter filter)
    {
        var result = await service.GetFilteredAnnouncements(filter);
        return Ok(result);
    }
    
    [HttpGet("get-realtors")]
    public async Task<IActionResult> GetRealtors([FromQuery] AnalyticsFilter filter)
    {
        var result = await service.GetRealtors(filter);
        return Ok(result);
    }
}