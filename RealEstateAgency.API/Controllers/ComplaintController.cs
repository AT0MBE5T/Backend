using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dto;
using RealEstateAgency.API.Mapper;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Services;
using RealEstateAgency.Application.Utils;

namespace RealEstateAgency.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ComplaintController(IComplaintService complaintService): ControllerBase
{
    [HttpPost("add-complaint")]
    public async Task<IActionResult> AddComplaint([FromBody] ComplaintRequest request)
    {
        var userId = User.GetUserId();
        
        if (userId == Guid.Empty)
            return Unauthorized();
        
        var viewDto = new ComplaintDto
        {
            UserId = userId,
            AnnouncementId = request.AnnouncementId,
            CreatedAt = DateTime.UtcNow,
            TypeId = request.TypeId,
            UserNote = request.UserNote,
            StatusId = Guid.Parse(StatusTypes.Pending)
        };
        
        var result = await complaintService.InsertAsync(viewDto);
        
        return result == Guid.Empty
            ? StatusCode(StatusCodes.Status500InternalServerError)
            : StatusCode(StatusCodes.Status201Created);
    }
    
    [HttpPost("update-complaint")]
    public async Task<IActionResult> UpdateComplaint([FromBody] ComplaintAdminRequest request)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var oldComplaint = await complaintService.GetByIdAsync(request.ComplaintId);

        if (oldComplaint is null)
            return NotFound();
        
        var viewDto = new ComplaintDto
        {
            Id = request.ComplaintId,
            UserId = oldComplaint.UserId,
            AnnouncementId = oldComplaint.AnnouncementId,
            CreatedAt = oldComplaint.CreatedAt,
            TypeId = oldComplaint.TypeId,
            UserNote = oldComplaint.UserNote,
            StatusId = request.StatusId,
            AdminId = request.AdminId,
            AdminNote =  request.AdminNote,
            ProcessedAt = DateTime.UtcNow
        };
        
        var result = await complaintService.UpdateAsync(viewDto);

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
            var result = await complaintService.GetAllComplaints();
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
            var result = await complaintService.GetAllOpenedComplaints();
            return Ok(result);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpGet("get-by-user-id/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var currentUserId = User.GetUserId();
        
        if (currentUserId != userId && !User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        try
        {
            var result = await complaintService.GetComplaintsByUserId(userId);
            return Ok(result);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}