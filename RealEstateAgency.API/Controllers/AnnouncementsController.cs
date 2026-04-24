using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dtos.Requests;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using AnnouncementRequest = RealEstateAgency.API.Dtos.Requests.AnnouncementRequest;
using ApiMapper = RealEstateAgency.API.Mappers.ApiMapper;

namespace RealEstateAgency.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AnnouncementsController(
    IAnnouncementsService announcementService,
    ApiMapper mapper): ControllerBase
{
    [AllowAnonymous]
    [HttpGet("get-announcement-full-by-id/{announcementId:guid}")]
    public async Task<IActionResult> GetAnnouncementFullById(Guid announcementId)
    {
        var userId = User.GetUserId();
        
        var command = new AnnouncementInfoCommandDto(announcementId, userId);
        
        var announcementFull = await announcementService.GetAnnouncementFullById(command);

        if (announcementFull == null)
        {
            return NotFound();
        }
        
        return Ok(announcementFull);
    }
    
    [HttpGet("get-announcements-grid")]
    public async Task<IActionResult> GetAnnouncementsGrid()
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var result = await announcementService.GetAnnouncementsGrid();
        return Ok(result);
    }
    
    [HttpPost("get-announcement-for-edit")]
    public async Task<IActionResult> GetAnnouncementForEdit([FromBody]Guid id)
    {
        if (User.IsInRole(Roles.USER))
            return Unauthorized();
        
        var announcementData = await announcementService.GetAnnouncementForEditByIdAsync(id);

        if (announcementData == null)
        {
            return NotFound();
        }
        
        return Ok(announcementData);
    }

    [HttpPost("add-announcement")]
    public async Task<IActionResult> AddAnnouncement([FromForm] AnnouncementRequest request)
    {
        if (!User.IsInRole(Roles.REALTOR) && !User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        if (request.Photos.Count == 0)
            return BadRequest("No files uploaded");

        var userId = User.GetUserId();
        
        var photos = request.Photos.Select(p => 
            new FileSourceDto(p.OpenReadStream(), p.FileName)).ToList();

        var command = new CreateAnnouncementCommandDto(
            userId,
            request.PropertyType,
            request.StatementType,
            request.Location,
            request.Area,
            request.Floors,
            request.Rooms,
            request.Title,
            request.Price,
            request.Content,
            request.Description,
            photos
            );
    
        var result = await announcementService.CreateAnnouncementAsync(command);
        
        foreach (var photo in photos) await photo.Content.DisposeAsync();

        return result.StatusCode == (int)HttpStatusCode.Created 
            ? Ok(result.Result) 
            : BadRequest(result.Error);
    }
    
    [HttpPost("update-announcement")]
    public async Task<IActionResult> UpdateAnnouncement([FromForm] AnnouncementEditRequest request)
    {
        if (!User.IsInRole(Roles.REALTOR) && !User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var userId = User.GetUserId();
        if (userId == Guid.Empty)
        {
            return Unauthorized();
        }
        
        var newPhotos = request.NewPhotos.Select(p => 
            new FileSourceDto(p.OpenReadStream(), p.FileName)).ToList();

        var command = new AnnouncementUpdateCommandDto(
            request.AnnouncementId,
            newPhotos,
            request.DeletedImageIds,
            request.ExistingImageOrder,
            request.PropertyType,
            request.StatementType,
            request.Location,
            request.Area,
            request.Floors,
            request.Rooms,
            request.Title,
            request.Price,
            request.Content,
            request.Description,
            userId
        );

        var result = await announcementService.UpdateAnnouncementAsync(command);
        
        foreach (var photo in newPhotos) await photo.Content.DisposeAsync();

        return result == string.Empty
            ? Ok() 
            : BadRequest(result);
    }
    
    [HttpPost("delete-announcement-by-id")]
    public async Task<IActionResult> DeleteAnnouncement([FromBody] Guid announcementId)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var userId = User.GetUserId();
        if (userId == Guid.Empty)
            return Unauthorized();
        
        var command = new DeleteAnnouncementCommandDto(userId, announcementId);
        var result = await announcementService.DeleteAnnouncementAsync(command);
        
        return result == string.Empty
            ? Ok()
            : BadRequest(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchRequest request)
    {
        var userId = User.GetUserId();
        var announcements = await announcementService.GetSearchDataPaginated(request.Text, request.Filters, request.SortId, request.Page, request.Limit, userId);
        var res = mapper.ListAnnouncementsShortAndPagesToListAnnouncementResponse(announcements.Data);
        var response = mapper.ToAnnouncementsResponseAndPages(
            announcements,
            res
        );
        
        return Ok(response);
    }
    
    [HttpPost("close-announcement")]
    public async Task<IActionResult> BuyPropertyByAnnouncement([FromBody] BuyRequest request)
    {
        if (!User.IsInRole(Roles.REALTOR) && !User.IsInRole(Roles.ADMIN))
            return Unauthorized();

        var command = new CloseAnnouncementCommandDto(request.CustomerId, request.AnnouncementId);
        
        var result = await announcementService.CloseAnnouncement(command);
        
        return result.Error == string.Empty
            ? Ok(result.Result)
            : BadRequest();
    }
    
    [HttpGet("get-bought")]
    public async Task<IActionResult> GetBoughtAnnouncements(
        [FromQuery] int page, 
        [FromQuery] int pageSize)
    {
        var userId = User.GetUserId();
        var announcements = await announcementService.GetBoughtAnnouncementsByUserId(userId, page, pageSize);
        var res = mapper.ListAnnouncementsShortAndPagesToListAnnouncementResponse(announcements.Data);
        var response = mapper.ToAnnouncementsResponseAndPages(
            announcements,
            res
        );
        
        return Ok(response);
    }
    
    [HttpGet("get-bought-by-user-id")]
    public async Task<IActionResult> GetBoughtAnnouncementsByUserId(
        [FromQuery] Guid userId,
        [FromQuery] int page, 
        [FromQuery] int pageSize)
    {
        var isAdmin = User.IsInRole(Roles.ADMIN);
        if (page < 1 || !isAdmin)
            return BadRequest();
            
        var announcements = await announcementService.GetBoughtAnnouncementsByUserId(userId, page, pageSize);
        var res = mapper.ListAnnouncementsShortAndPagesToListAnnouncementResponse(announcements.Data);
        var response = mapper.ToAnnouncementsResponseAndPages(
            announcements,
            res
        );
        
        return Ok(response);
    }
    
    [HttpGet("get-sold")]
    public async Task<IActionResult> GetSoldAnnouncements(
        [FromQuery] int page, 
        [FromQuery] int pageSize)
    {
        var userId = User.GetUserId();
        var announcements = await announcementService.GetSoldAnnouncementsByUserId(userId, page, pageSize);
        var res = mapper.ListAnnouncementsShortAndPagesToListAnnouncementResponse(announcements.Data);
        var response = mapper.ToAnnouncementsResponseAndPages(
            announcements,
            res
        );
        
        return Ok(response);
    }
    
    [HttpGet("get-sold-by-user-id")]
    public async Task<IActionResult> GetSoldAnnouncementsByUserId(
        [FromQuery] Guid userId,
        [FromQuery] int page, 
        [FromQuery] int pageSize)
    {
        var isAdmin = User.IsInRole(Roles.ADMIN);
        if (page < 1 || !isAdmin)
            return BadRequest();
        
        var announcements = await announcementService.GetSoldAnnouncementsByUserId(userId, page, pageSize);
        var res = mapper.ListAnnouncementsShortAndPagesToListAnnouncementResponse(announcements.Data);
        var response = mapper.ToAnnouncementsResponseAndPages(
            announcements,
            res
        );
        
        return Ok(response);
    }
    
    [HttpGet("get-placed")]
    public async Task<IActionResult> GetPlacedAnnouncements(
        [FromQuery] int page, 
        [FromQuery] int pageSize)
    {
        var userId = User.GetUserId();
        
        if (page < 1)
            return BadRequest();
        
        var announcements = await announcementService.GetPlacedAnnouncementsByUserId(userId, page, pageSize);
        var res = mapper.ListAnnouncementsShortAndPagesToListAnnouncementResponse(announcements.Data);
        var response = mapper.ToAnnouncementsResponseAndPages(
            announcements,
            res
        );
        
        return Ok(response);
    }
    
    [HttpGet("get-placed-by-user-id")]
    public async Task<IActionResult> GetPlacedAnnouncementsByUserId(
        [FromQuery] Guid userId,
        [FromQuery] int page, 
        [FromQuery] int pageSize)
    {
        var userRoles = User.IsInRole(Roles.ADMIN);
        if (page < 1 || !userRoles)
            return BadRequest();
        
        var announcements = await announcementService.GetPlacedAnnouncementsByUserId(userId, page, pageSize);
        var res = mapper.ListAnnouncementsShortAndPagesToListAnnouncementResponse(announcements.Data);
        var response = mapper.ToAnnouncementsResponseAndPages(
            announcements,
            res
        );
        
        return Ok(response);
    }
    
    [HttpPost("switch-verification")]
    public async Task<IActionResult> SwitchVerify([FromBody] Guid announcementId)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var userId =  User.GetUserId();
        if (userId == Guid.Empty)
            return Unauthorized();
        
        var command = new SwitchVerificationCommandDto(userId, announcementId);
        var error = await announcementService.SwitchVerifyAnnouncement(command);
        
        return error == string.Empty
            ? Ok()
            : NotFound(error);
    }
}