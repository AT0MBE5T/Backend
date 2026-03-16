using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dto;
using RealEstateAgency.API.Mapper;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Services;
using RealEstateAgency.Application.Utils;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComplaintController(IComplaintService complaintService, ApiMapper mapper): ControllerBase
{
    [HttpPost("add-complaint")]
    public async Task<IActionResult> AddComplaint([FromBody] ComplaintRequest request)
    {
        var viewDto = new ComplaintDto
        {
            UserId = request.UserId,
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
}