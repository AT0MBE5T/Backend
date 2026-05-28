using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dtos.Requests;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Entities;

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
    public async Task<IActionResult> Subscribe([FromBody] PushSubscriptionRequest request)
    {
        if (request?.Keys == null || string.IsNullOrWhiteSpace(request.Endpoint))
        {
            return BadRequest("Invalid subscription data");
        }

        var userId = User.GetUserId();

        if (userId == Guid.Empty)
            return Unauthorized();
        
        var existingSubscription = await _service.GetByEndpointAsync(request.Endpoint);
        
        if (existingSubscription != null)
        {
            existingSubscription.UserId = userId;
            existingSubscription.P256DH = request.Keys.P256dh;
            existingSubscription.Auth = request.Keys.Auth;

            await _service.UpdateAsync(existingSubscription);

            return Ok(new { message = "Subscription updated" });
        }
        
        await _service.AddAsync(new UserPushSubscription
        {
            UserId = userId,
            Endpoint = request.Endpoint,
            P256DH = request.Keys.P256dh,
            Auth = request.Keys.Auth
        });

        return Ok(new { message = "Subscription created" });
    }
    
    [HttpPost("unsubscribe")]
    public async Task<IActionResult> Unsubscribe([FromBody] string endpoint)
    {
        var subscription = await _service.GetByEndpointAsync(endpoint);

        if (subscription != null)
        {
            await _service.RemoveByIdAsync(subscription.Id);
        }

        return Ok();
    }
}