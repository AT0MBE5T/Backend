using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dtos.Requests;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Services;
using RealEstateAgency.Application.Utils;

namespace RealEstateAgency.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SupportsController(ISupportService supportService): ControllerBase
{
    [HttpPost("add-support")]
    public async Task<IActionResult> AddSupport([FromBody] SupportRequest request)
    {
        var userId = User.GetUserId();
        
        if (userId == Guid.Empty)
            return Unauthorized();
        
        var supportDto = new SupportDto
        {
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UserNote = request.UserNote,
        };
        
        var result = await supportService.InsertAsync(supportDto);

        return result == Guid.Empty
            ? BadRequest()
            : Created();
    }
    
    [HttpPost("update-support-admin")]
    public async Task<IActionResult> UpdateSupportAdmin([FromBody] Guid supportId)
    {
        var userId = User.GetUserId();
        
        if (userId == Guid.Empty || !User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var oldSupport = await supportService.GetByIdAsync(supportId);

        if (oldSupport is null)
            return NotFound();
        
        var result = await supportService.AdminJoinAsync(supportId, userId);

        return result is not null && result != Guid.Empty
            ? Ok(result)
            : BadRequest();
    }
    
    [HttpPost("close-support")]
    public async Task<IActionResult> CloseSupport([FromBody] Guid supportId)
    {
        var userId = User.GetUserId();
        
        if (userId == Guid.Empty || !User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var oldSupport = await supportService.GetByIdAsync(supportId);

        if (oldSupport is null)
            return NotFound();
        
        var result = await supportService.CloseAsync(supportId);

        return result
            ? Ok()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        try
        {
            var result = await supportService.GetAllSupports();
            return Ok(result);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpGet("get-opened")]
    public async Task<IActionResult> GetOpened()
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        try
        {
            var result = await supportService.GetAllOpenedSupports();
            return Ok(result);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}