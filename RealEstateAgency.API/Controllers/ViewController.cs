using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dto;
using RealEstateAgency.API.Mapper;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Services;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ViewController(IViewService viewService): ControllerBase
{
    [HttpPost("add-view")]
    public async Task<IActionResult> AddView([FromBody] ViewRequest request)
    {
        var viewDto = new ViewDto
        {
            UserId = request.UserId,
            AnnouncementId = request.AnnouncementId,
            CreatedAt = DateTime.UtcNow
        };
        
        var result = await viewService.InsertAsync(viewDto);
        
        return result == Guid.Empty
            ? StatusCode(StatusCodes.Status500InternalServerError)
            : StatusCode(StatusCodes.Status201Created);
    }
    
    [HttpPost("delete-view")]
    public async Task<IActionResult> DeleteView([FromBody] ViewRequest request)
    {
        var viewDto = new ViewDto
        {
            UserId = request.UserId,
            AnnouncementId = request.AnnouncementId,
            CreatedAt = DateTime.UtcNow
        };
        
        var result = await viewService.DeleteAsync(viewDto);
        
        return !result
            ? StatusCode(StatusCodes.Status500InternalServerError)
            : StatusCode(StatusCodes.Status201Created);
    }
}