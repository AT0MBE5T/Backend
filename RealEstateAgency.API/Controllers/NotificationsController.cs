using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dtos.Requests;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.API.Controllers;

[Authorize]
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
    public async Task<IActionResult> Subscribe([FromBody] PushSubscriptionRequest request)
    {
        if (request?.Keys == null) 
        {
            return BadRequest("Invalid subscription data");
        }

        var userId = User.GetUserId();
        var existingSubs = await _service.GetAllByUserIdAsync(userId);
    
        if (existingSubs.Any(s => s.Endpoint == request.Endpoint))
        {
            return Ok(new { message = "Подписка уже существует" });
        }

        await _service.AddAsync(new UserPushSubscription {
            UserId = userId,
            Endpoint = request.Endpoint,
            P256DH = request.Keys.P256dh,
            Auth = request.Keys.Auth
        });

        return Ok();
    }
}