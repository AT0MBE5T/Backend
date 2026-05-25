using Microsoft.AspNetCore.SignalR;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Interfaces;

public interface IMessageHub
{
    Task JoinChat(UserConnection connection);
    Task JoinCommonChat(string userName);
    Task JoinRoom(UserConnection connection);
    Task JoinRoomWpf(UserConnection connection);
    Task OnDisconnectedAsync(Exception? exception);
    Task NotifyUpdateFullOfferAsync(AnnouncementFullDto offerDto);
    Task SendMessage(Guid chatId, string message, string userName, Guid? offerId);
    Task SendMessageInCommon(string message, string userName);
    Task LeaveComment(Guid chatId, string message, string userName);
    Task SendQuestion(Guid chatId, string message, string userName);
    Task SendAnswer(Guid chatId, Guid questionId, string message, string userName);
    Task AddOffer(Guid chatId, AnnouncementShortDto offer);
    Task UpdateOffer(Guid chatId, AnnouncementShortDto offer);
    Task DeleteOffer(Guid chatId, Guid offerId);
    Task DeleteComment(Guid chatId, Guid commentId);
    Task DeleteAnswer(Guid chatId, Guid answerId);
    Task DeleteQuestion(Guid chatId, Guid questionId);
    Task OnConnectedAsync();
    Task UpdateFullOffer(AnnouncementFullDto offer);
    void Dispose();
    IHubCallerClients<IChatClient> Clients { get; set; }
    HubCallerContext Context { get; set; }
    IGroupManager Groups { get; set; }
}