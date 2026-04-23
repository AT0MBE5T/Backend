using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Mapper;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Services;
using RealEstateAgency.Application.Utils;

namespace RealEstateAgency.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CommentController(
    ICommentsService commentsService,
    ApiMapper mapper): ControllerBase
{
    [AllowAnonymous]
    [HttpGet("get-comments-by-announcement-id/{chatId:guid}")]
    public async Task<IActionResult> GetCommentsByAnnouncementId(Guid chatId)
    {
        var comments = await commentsService.GetAllByAnnouncementId(chatId);
        var result = comments
            .Select(mapper.CommentDtoToCommentResponse).ToList();
        
        return Ok(result);
    }
    
    [HttpGet("get-comments-grid")]
    public async Task<IActionResult> GetCommentsGrid()
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var result = await commentsService.GetCommentsGrid();
        return Ok(result);
    }

    [HttpPost("add-comment")]
    public async Task<IActionResult> AddComment([FromBody] CommentDto commentDto)
    {
        var commentId = await commentsService.InsertCommentAsync(commentDto);
        
        return commentId == Guid.Empty
            ? StatusCode(StatusCodes.Status500InternalServerError)
            : StatusCode(StatusCodes.Status201Created);
    }

    [HttpPost("delete-comment-by-id")]
    public async Task<IActionResult> DeleteCommentById([FromBody]Guid commentId)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        return await commentsService.DeleteByCommentIdAsync(commentId)
            ? Ok()
            : BadRequest();
    }
}