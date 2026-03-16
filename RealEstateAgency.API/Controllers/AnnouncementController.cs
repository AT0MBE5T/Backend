using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealEstateAgency.API.Dto;
using RealEstateAgency.API.Mapper;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Hubs;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementController(
    IAnnouncementsService announcementService,
    IImageService imageService,
    IPropertyService propertyService,
    IStatementsService statementService,
    IPaymentService paymentService,
    UserManager<User> userManager,
    IHubContext<MessageHub> hubContext,
    ApiMapper mapper): ControllerBase
{

    [HttpGet("get-pages")]
    public async Task<IActionResult> GetPages()
    {
        var pages = await announcementService.GetPages();
        return pages > 0 
            ? Ok(pages) 
            : NoContent();
    }

    [HttpGet("get-announcement-full-by-id/{announcementId:guid}")]
    public async Task<IActionResult> GetAnnouncementFullById(Guid announcementId)
    {
        var userId = User.GetUserId();
        
        var announcementFull = await announcementService.GetAnnouncementFullById(announcementId, userId);

        if (announcementFull == null)
        {
            return NotFound();
        }
        
        return Ok(announcementFull);
    }
    
    [HttpPost("get-announcement-for-edit")]
    public async Task<IActionResult> GetAnnouncementForEdit([FromBody]Guid id)
    {
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
        var userId = User.GetUserId();
        
        var photos = request.Photos;
        
        if (photos.Count == 0)
            return BadRequest("No file uploaded");

        var property = mapper.AnnouncementRequestToPropertyDto(request);
        var propertyId = await propertyService.AddProperty(property);

        for (var i = 0; i < photos.Count; ++i)
        {
            var imageResult = await imageService.UploadImageAsync(photos[i]);
        
            if (imageResult.Error != null) 
                throw new Exception(imageResult.Error.Message);

            await imageService.InsertAsync(new ImageDto
            {
                PhotoUrl = imageResult.SecureUrl.ToString(),
                PublicId = imageResult.PublicId,
                PropertyId = propertyId,
                OrderIndex = i
            });
        }
        
        var statement = mapper.AnnouncementRequestToStatementDto(request, propertyId, DateTime.UtcNow);
        var statementId = await statementService.AddStatementAsync(statement);

        if (statementId == null)
        {
            return NotFound();
        }

        var announcement = mapper.AnnouncementRequestToAnnouncementDto(request, (Guid)statementId, true);
        var announcementId = await announcementService.AddAnnouncement(request.UserId, announcement);

        var offerDto = await announcementService.GetAnnouncementShortByOfferId(announcementId!.Value, userId);
        
        await hubContext.Clients.Group("offers_global")
            .SendAsync("ReceiveOffer", offerDto);
        
        return Ok(announcementId);
    }
    
    [HttpPost("update-announcement")]
    public async Task<IActionResult> UpdateAnnouncement([FromForm] AnnouncementEditRequest request)
    {
        var userId = User.GetUserId();
        
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        
        var statementId = await announcementService.GetStatementIdByAnnouncementIdAsync(request.AnnouncementId);
        var propertyId = await announcementService.GetPropertyIdByAnnouncementIdAsync(request.AnnouncementId);

        if (statementId == null || propertyId == null)
        {
            return NotFound();
        }
        
        var photos = request.NewPhotos?.ToList() ?? new List<IFormFile>();
        var deleteImages = request.DeletedImageIds ?? [];
        var existingImageOrder = request.ExistingImageOrder ?? [];

        foreach (var id in deleteImages)
        {
            await imageService.DeleteAsync(id);
        }

        var newPhotoIndex = 0;
        var orderIndex = 0;

        foreach (var item in existingImageOrder)
        {
            if (item == "new")
            {
                var file = photos[newPhotoIndex];
                newPhotoIndex++;

                var imageResult = await imageService.UploadImageAsync(file);

                if (imageResult.Error != null)
                    throw new Exception(imageResult.Error.Message);

                await imageService.InsertAsync(new ImageDto
                {
                    PhotoUrl = imageResult.SecureUrl.ToString(),
                    PublicId = imageResult.PublicId,
                    PropertyId = propertyId,
                    OrderIndex = orderIndex
                });
            }
            else
            {
                await imageService.UpdateOrderAsync(Guid.Parse(item), orderIndex);
            }

            orderIndex++;
        }

        var property = mapper.AnnouncementEditRequestToPropertyDto(request);
        var propertyRes = await propertyService.UpdatePropertyAsync((Guid)propertyId, property);
        
        if (!propertyRes)
        {
            return NotFound();
        }
        
        var statement = mapper.AnnouncementEditRequestToStatementDto(request, (Guid)propertyId, DateTime.UtcNow);
        var statementRes = await statementService.UpdateStatementAsync((Guid)statementId, user.Id, statement);
        
        if (!statementRes)
        {
            return NotFound();
        }
        
        var announcement = mapper.AnnouncementEditRequestToAnnouncementDto(request, (Guid)statementId, true);
        var announcementId = await announcementService.UpdateAnnouncementAsync(request.AnnouncementId, user.Id, announcement);
        
        var offerDto = await announcementService.GetAnnouncementShortByOfferId(request.AnnouncementId, userId);
        
        await hubContext.Clients.Group("offers_global")
            .SendAsync("UpdateOffer", offerDto);
        
        return Ok(announcementId);
    }

    [HttpPost("delete-announcement-by-id")]
    public async Task<IActionResult> DeleteAnnouncement([FromBody] Guid announcementId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var result = await announcementService.DeleteAnnouncementAsync(announcementId, user.Id);
        
        await hubContext.Clients.Group("offers_global")
            .SendAsync("DeleteOffer", announcementId);
        
        return result
            ? Ok()
            : NotFound();
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchRequestDto request)
    {
        var announcements = await announcementService.GetSearchDataPaginated(request.Text, request.Filters, request.SortId, request.Page, request.Limit, request.UserId);
        var res = mapper.ListAnnouncementsShortAndPagesToListAnnouncementResponse(announcements.Data);
        var response = mapper.ToAnnouncementsResponseAndPages(
            announcements,
            res
        );
        
        return Ok(response);
    }

    [HttpPost("buy-property-by-announcement-id")]
    public async Task<IActionResult> BuyPropertyByAnnouncement([FromBody] BuyRequest request)
    {
        var paymentDto = mapper.BuyRequestToPaymentDto(request);
        var paymentId = await paymentService.InsertPayment(paymentDto);
        
        return paymentId == Guid.Empty
            ? NotFound()
            : Ok(paymentId);
    }
    
    [HttpGet("get-bought-by-user-id/{page:int}")]
    public async Task<IActionResult> GetBoughtAnnouncementsByUserId(int page)
    {
        var userId = User.GetUserId();
        var announcements = await announcementService.GetBoughtAnnouncementsByUserId(userId, page);
        var res = mapper.ListAnnouncementsShortAndPagesToListAnnouncementResponse(announcements.Data);
        var response = mapper.ToAnnouncementsResponseAndPages(
            announcements,
            res
        );
        
        return Ok(response);
    }
    
    [HttpGet("get-sold-by-user-id/{page:int}")]
    public async Task<IActionResult> GetSoldAnnouncementsByUserId(int page)
    {
        var userId = User.GetUserId();
        var announcements = await announcementService.GetSoldAnnouncementsByUserId(userId, page);
        var res = mapper.ListAnnouncementsShortAndPagesToListAnnouncementResponse(announcements.Data);
        var response = mapper.ToAnnouncementsResponseAndPages(
            announcements,
            res
        );
        
        return Ok(response);
    }
    
    [HttpGet("get-placed-by-user-id/{page:int}")]
    public async Task<IActionResult> GetPlacedAnnouncementsByUserId(int page)
    {
        var userId = User.GetUserId();
        var announcements = await announcementService.GetPlacedAnnouncementsByUserId(userId, page);
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
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        
        var res = await announcementService.SwitchVerificateAnnouncement(announcementId, user.Id);
        
        return res
                ? Ok()
                : NotFound();
    }
}