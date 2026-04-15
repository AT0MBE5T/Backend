using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Models;
using WebPush;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly IUserPushSubscriptionService _service;

    public NotificationsController(IUserPushSubscriptionService service)
    {
        _service = service;
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] PushSubscriptionDto dto)
    {
        if (dto?.Keys == null) 
        {
            return BadRequest("Invalid subscription data");
        }

        var userId = User.GetUserId();
        var existingSubs = await _service.GetAllByUserIdAsync(userId);
    
        if (existingSubs.Any(s => s.Endpoint == dto.Endpoint))
        {
            return Ok(new { message = "Подписка уже существует" });
        }

        await _service.AddAsync(new UserPushSubscription {
            UserId = userId,
            Endpoint = dto.Endpoint,
            P256DH = dto.Keys.P256dh,
            Auth = dto.Keys.Auth
        });

        return Ok();
    }
}