using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;

namespace RealEstateAgency.API.Controllers;

// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class ChatController(IChatService chatService): ControllerBase
{
    [HttpPost("get-or-create-chat")]
    public async Task<IActionResult> GetOrCreateChatRoom([FromBody] Guid authorId)
    {
        var userId = User.GetUserId();
        
        var chatId = await chatService.GetOrCreateChat(userId, authorId);
        return Ok(chatId);
    }
    
    [HttpGet("my-chats")]
    public async Task<IActionResult> GetChatsByUserId()
    {
        var userId = User.GetUserId();
        var chats = await chatService.GetChatsAsyncByUserId(userId);
        return Ok(chats);
    }
    
    [HttpGet("get-messages-by-chat-id/{chatId:guid}")]
    public async Task<IActionResult> GetMessagesByChatId(Guid chatId)
    {
        var messages = await chatService.GetMessagesByChatId(chatId);

        return Ok(messages);
    }
}