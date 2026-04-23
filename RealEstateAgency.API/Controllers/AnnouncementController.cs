using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dto;
using RealEstateAgency.API.Mapper;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using AnnouncementRequest = RealEstateAgency.API.Dto.AnnouncementRequest;

namespace RealEstateAgency.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AnnouncementController(
    IAnnouncementsService announcementService,
    ApiMapper mapper): ControllerBase
{
    [AllowAnonymous]
    [HttpGet("get-announcement-full-by-id/{announcementId:guid}")]
    public async Task<IActionResult> GetAnnouncementFullById(Guid announcementId)
    {
        var userId = User.GetUserId();
        
        var command = new AnnouncementInfoCommand(announcementId, userId);
        
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
    
    // [HttpPost("add-announcement")]
    // public async Task<IActionResult> AddAnnouncement([FromForm] AnnouncementRequest request)
    // {
    //     if (User.IsInRole(Roles.USER))
    //         return Unauthorized();
    //     
    //     var userId = User.GetUserId();
    //     
    //     var photos = request.Photos;
    //     
    //     if (photos.Count == 0)
    //         return BadRequest("No file uploaded");
    //
    //     var property = mapper.AnnouncementRequestToPropertyDto(request);
    //     var propertyId = await propertyService.AddProperty(property);
    //
    //     for (var i = 0; i < photos.Count; ++i)
    //     {
    //         if (photos[i].Length == 0)
    //             return BadRequest("Image is required");
    //
    //         await using var stream = photos[i].OpenReadStream();
    //         var imageResult = await imageService.UploadImageAsync(stream, photos[i].FileName);
    //     
    //         if (imageResult.Error != null) 
    //             throw new Exception(imageResult.Error);
    //
    //         await imageService.InsertAsync(new ImageDto
    //         {
    //             PhotoUrl = imageResult.Url,
    //             PublicId = imageResult.PublicId,
    //             PropertyId = propertyId,
    //             OrderIndex = i
    //         });
    //     }
    //     
    //     var statement = mapper.AnnouncementRequestToStatementDto(request, propertyId, DateTime.UtcNow);
    //     var statementId = await statementService.AddStatementAsync(statement);
    //
    //     if (statementId == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var announcement = mapper.AnnouncementRequestToAnnouncementDto(request, (Guid)statementId, true);
    //     var announcementId = await announcementService.AddAnnouncement(userId, announcement);
    //
    //     var offerDto = await announcementService.GetAnnouncementShortByOfferId(announcementId!.Value, userId);
    //     
    //     await hubContext.Clients.Group("offers_global")
    //         .SendAsync("ReceiveOffer", offerDto);
    //     
    //     return Ok(announcementId);
    // }
    
    [HttpPost("add-announcement")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IActionResult> AddAnnouncement([FromForm] AnnouncementRequest request)
    {
        if (request.Photos.Count == 0)
            return BadRequest("No files uploaded");

        var userId = User.GetUserId();
        
        var photos = request.Photos.Select(p => 
            new FileSource(p.OpenReadStream(), p.FileName)).ToList();

        var command = new CreateAnnouncementCommand(
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
    
    // [HttpPost("update-announcement")]
    // public async Task<IActionResult> UpdateAnnouncement([FromForm] AnnouncementEditRequest request)
    // {
    //     if (User.IsInRole(Roles.USER))
    //         return Unauthorized();
    //     
    //     var userId = User.GetUserId();
    //     
    //     var user = await userManager.GetUserAsync(User);
    //     if (user == null)
    //     {
    //         return Unauthorized();
    //     }
    //     
    //     var statementId = await announcementService.GetStatementIdByAnnouncementIdAsync(request.AnnouncementId);
    //     var propertyId = await announcementService.GetPropertyIdByAnnouncementIdAsync(request.AnnouncementId);
    //
    //     if (statementId == null || propertyId == null)
    //     {
    //         return NotFound();
    //     }
    //     
    //     var photos = request.NewPhotos?.ToList() ?? new List<IFormFile>();
    //     var deleteImages = request.DeletedImageIds ?? [];
    //     var existingImageOrder = request.ExistingImageOrder ?? [];
    //
    //     foreach (var id in deleteImages)
    //     {
    //         await imageService.DeleteAsync(id);
    //     }
    //
    //     var newPhotoIndex = 0;
    //     var orderIndex = 0;
    //
    //     foreach (var item in existingImageOrder)
    //     {
    //         if (item == "new")
    //         {
    //             var file = photos[newPhotoIndex];
    //             newPhotoIndex++;
    //             
    //             if (file.Length == 0)
    //                 return BadRequest("Image is required");
    //
    //             await using var stream = file.OpenReadStream();
    //             var imageResult = await imageService.UploadImageAsync(stream, file.FileName);
    //
    //             if (imageResult.Error != null)
    //                 throw new Exception(imageResult.Error);
    //
    //             await imageService.InsertAsync(new ImageDto
    //             {
    //                 PhotoUrl = imageResult.Url,
    //                 PublicId = imageResult.PublicId,
    //                 PropertyId = propertyId,
    //                 OrderIndex = orderIndex
    //             });
    //         }
    //         else
    //         {
    //             await imageService.UpdateOrderAsync(Guid.Parse(item), orderIndex);
    //         }
    //
    //         orderIndex++;
    //     }
    //
    //     var property = mapper.AnnouncementEditRequestToPropertyDto(request);
    //     var propertyRes = await propertyService.UpdatePropertyAsync((Guid)propertyId, property);
    //     
    //     if (!propertyRes)
    //     {
    //         return NotFound();
    //     }
    //     
    //     var statement = mapper.AnnouncementEditRequestToStatementDto(request, (Guid)propertyId, DateTime.UtcNow);
    //     var statementRes = await statementService.UpdateStatementAsync((Guid)statementId, user.Id, statement);
    //     
    //     if (!statementRes)
    //     {
    //         return NotFound();
    //     }
    //     
    //     var announcement = mapper.AnnouncementEditRequestToAnnouncementDto(request, (Guid)statementId, true);
    //     var announcementId = await announcementService.UpdateAnnouncementAsync(request.AnnouncementId, user.Id, announcement);
    //     
    //     var offerDto = await announcementService.GetAnnouncementShortByOfferId(request.AnnouncementId, userId);
    //     
    //     await hubContext.Clients.Group("offers_global")
    //         .SendAsync("UpdateOffer", offerDto);
    //     
    //     return Ok(announcementId);
    // }
    
    [HttpPost("update-announcement")]
    public async Task<IActionResult> UpdateAnnouncement([FromForm] AnnouncementEditRequest request)
    {
        if (User.IsInRole(Roles.USER))
            return Unauthorized();
        
        var userId = User.GetUserId();
        if (userId == Guid.Empty)
        {
            return Unauthorized();
        }
        
        var newPhotos = request.NewPhotos.Select(p => 
            new FileSource(p.OpenReadStream(), p.FileName)).ToList();

        var command = new AnnouncementUpdateCommand(
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
        
        var command = new DeleteAnnouncementCommand(userId, announcementId);
        var result = await announcementService.DeleteAnnouncementAsync(command);
        
        return result == string.Empty
            ? Ok()
            : BadRequest(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchRequestDto request)
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
        if (User.IsInRole(Roles.USER))
            return Unauthorized();

        var command = new CloseAnnouncementCommand(request.CustomerId, request.AnnouncementId);
        
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
        
        var command = new SwitchVerificationCommand(userId, announcementId);
        var error = await announcementService.SwitchVerifyAnnouncement(command);
        
        return error == string.Empty
            ? Ok()
            : NotFound(error);
    }
}