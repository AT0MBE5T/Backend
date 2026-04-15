using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RealEstateAgency.Application.Interfaces.Services;
using WebPush;

namespace RealEstateAgency.Infrastructure.Hubs;

public class WebPushService(IConfiguration _configuration, IUserPushSubscriptionService _userPushSubscriptionService)
{
    public async Task SendNotificationToUserAsync(Guid userId, string message, string? url = null, string? title = null)
    {
        var subscriptions = await _userPushSubscriptionService.GetAllByUserIdAsync(userId);
    
        var vapidDetails = new VapidDetails(
            _configuration["VapidDetails:Subject"],
            _configuration["VapidDetails:PublicKey"],
            _configuration["VapidDetails:PrivateKey"]
        );

        var webPushClient = new WebPushClient();
        var payload = JsonSerializer.Serialize(new {
            title = title ?? "",
            body = message,
            url = url ?? "/" 
        });

        foreach (var sub in subscriptions)
        {
            var pushSub = new PushSubscription(sub.Endpoint, sub.P256DH, sub.Auth);

            try
            {
                await webPushClient.SendNotificationAsync(pushSub, payload, vapidDetails);
            }
            catch (WebPushException ex) when (ex.StatusCode == HttpStatusCode.Gone || ex.StatusCode == HttpStatusCode.NotFound)
            {
                await _userPushSubscriptionService.RemoveByIdAsync(sub.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке Push: {ex.Message}");
            }
        }
    }
    
    // public async Task SendPush(PushSubscription subscription, string message)
    // {
    //     var vapidDetails = new VapidDetails(
    //         _configuration["VapidDetails:Subject"],
    //         _configuration["VapidDetails:PublicKey"],
    //         _configuration["VapidDetails:PrivateKey"]
    //     );
    //
    //     var webPushClient = new WebPushClient();
    //
    //     var payload = JsonSerializer.Serialize(new {
    //         title = "Новое сообщение",
    //         body = message,
    //         url = "/chats" 
    //     });
    //
    //     try
    //     {
    //         await webPushClient.SendNotificationAsync(subscription, payload, vapidDetails);
    //     }
    //     catch (WebPushException ex)
    //     {
    //         if (ex.StatusCode == System.Net.HttpStatusCode.Gone || ex.StatusCode == System.Net.HttpStatusCode.NotFound)
    //         {
    //             // Подписка больше не валидна (пользователь удалил PWA или отозвал права)
    //             // Удалите её из вашей базы данных
    //         }
    //         Console.WriteLine($"Ошибка Push: {ex.Message}");
    //     }
    // }
}