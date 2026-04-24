using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dtos.Requests;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using ApiMapper = RealEstateAgency.API.Mappers.ApiMapper;

namespace RealEstateAgency.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FavoritesController(
    IFavoriteService favoriteService,
    ApiMapper mapper): ControllerBase
{
    [HttpGet("get-favorites")]
    public async Task<IActionResult> GetFavoriteAnnouncements(
        [FromQuery] int page, 
        [FromQuery] int pageSize)
    {
        var userId = User.GetUserId();
        var announcements = await favoriteService.GetFavoritesByUserId(userId, page, pageSize);
        var res = mapper.ListAnnouncementsShortAndPagesToListAnnouncementResponse(announcements.Data);
        var response = mapper.ToAnnouncementsResponseAndPages(
            announcements,
            res
        );
        
        return Ok(response);
    }
    
    [HttpGet("get-favorites-by-user-id")]
    public async Task<IActionResult> GetFavoriteAnnouncementsByUserId(
        [FromQuery] Guid userId,
        [FromQuery] int page, 
        [FromQuery] int pageSize)
    {
        var isAdmin = User.IsInRole(Roles.ADMIN);
        if (page < 1 || !isAdmin)
            return BadRequest();
            
        var announcements = await favoriteService.GetFavoritesByUserId(userId, page, pageSize);
        var res = mapper.ListAnnouncementsShortAndPagesToListAnnouncementResponse(announcements.Data);
        var response = mapper.ToAnnouncementsResponseAndPages(
            announcements,
            res
        );
        
        return Ok(response);
    }

    [HttpPost("add-favorite")]
    public async Task<IActionResult> AddFavorite([FromBody] FavoriteRequest request)
    {
        var userId =  User.GetUserId();

        var favoriteDto = new FavoriteDto
        {
            AnnouncementId = request.AnnouncementId,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };
        var error = await favoriteService.AddComplaintAsync(favoriteDto);
        
        return error == string.Empty
            ? Created()
            : BadRequest(error);
    }
    
    [HttpPost("delete-favorite")]
    public async Task<IActionResult> DeleteFavorite([FromBody] FavoriteRequest request)
    {
        var userId =  User.GetUserId();
        
        var favoriteDto = new FavoriteDto
        {
            UserId = userId,
            AnnouncementId = request.AnnouncementId,
            CreatedAt = DateTime.UtcNow
        };
        
        var error = await favoriteService.DeleteByDtoAsync(favoriteDto);
        
        return error == string.Empty
            ? Created()
            : BadRequest(error);
    }
}