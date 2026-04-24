using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dtos.Requests;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;

namespace RealEstateAgency.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ViewsController(IViewService viewService): ControllerBase
{
    [HttpPost("add-view")]
    public async Task<IActionResult> AddView([FromBody] ViewRequest request)
    {
        var userId = User.GetUserId();
        var viewDto = new ViewDto
        {
            UserId = userId,
            AnnouncementId = request.AnnouncementId,
            CreatedAt = DateTime.UtcNow
        };
        
        var result = await viewService.InsertAsync(viewDto);
        
        return result == Guid.Empty
            ? BadRequest()
            : Created();
    }
}