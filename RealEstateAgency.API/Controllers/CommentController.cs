using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Mapper;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Services;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController(ICommentsService commentsService, IAccountService accountService, ApiMapper mapper): ControllerBase
{
    [HttpGet("get-comments-by-announcement-id/{chatId:guid}")]
    public async Task<IActionResult> GetCommentsByAnnouncementId(Guid chatId)
    {
        var comments = await commentsService.GetAllByAnnouncementId(chatId);
        var tasks = comments
            .Select(async x =>
            {
                var authorName = await accountService.GetNameSurnameById(x.UserId);
                return mapper.CommentDtoToCommentResponse(x, authorName);
            }).ToList();

        var result = await Task.WhenAll(tasks);
        
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
        return await commentsService.DeleteByCommentIdAsync(commentId)
            ? Ok()
            : BadRequest();
    }
}