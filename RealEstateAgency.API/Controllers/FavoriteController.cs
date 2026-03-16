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
public class FavoriteController(IFavoriteService favoriteService, ApiMapper mapper): ControllerBase
{
    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchFavoritesRequestDto request)
    {
        var announcements = await favoriteService.GetSearchDataAsync(request.UserId, request.Text, request.Filters, request.SortId, request.Page, request.Limit);
        var res = mapper.ListAnnouncementsShortAndPagesToListAnnouncementResponse(announcements.Data);
        var response = mapper.ToAnnouncementsResponseAndPages(
            announcements,
            res
        );
        
        return Ok(response);
    }
    
    [HttpGet("get-favorites-by-user-id/{page:int}")]
    public async Task<IActionResult> GetSoldAnnouncementsByUserId(int page)
    {
        var userId = User.GetUserId();
        var announcements = await favoriteService.GetFavoritesByUserId(userId, page);
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
        var favoriteDto = new FavoriteDto
        {
            UserId = request.UserId,
            AnnouncementId = request.AnnouncementId,
            CreatedAt = DateTime.UtcNow
        };
        
        var result = await favoriteService.AddAsync(favoriteDto);
        
        return !result
            ? StatusCode(StatusCodes.Status500InternalServerError)
            : StatusCode(StatusCodes.Status201Created);
    }
    
    [HttpPost("delete-favorite")]
    public async Task<IActionResult> DeleteFavorite([FromBody] FavoriteRequest request)
    {
        var favoriteDto = new FavoriteDto
        {
            UserId = request.UserId,
            AnnouncementId = request.AnnouncementId,
            CreatedAt = DateTime.UtcNow
        };
        
        var result = await favoriteService.DeleteByDtoAsync(favoriteDto);
        
        return !result
            ? StatusCode(StatusCodes.Status500InternalServerError)
            : StatusCode(StatusCodes.Status201Created);
    }
}