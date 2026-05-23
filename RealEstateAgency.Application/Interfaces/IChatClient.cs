using RealEstateAgency.Core.Dtos;

namespace RealEstateAgency.Application.Interfaces;

public interface IChatClient
{
    public Task ReceiveMessage(Guid userId, string userName, string message, Guid chatId);
    public Task ReceiveSupportWPF(SupportGridDto support);
    public Task UpdateSupportWPF(SupportGridDto support);
    public Task ReceiveComplaintWPF(ComplaintGridDto complaint);
    public Task UpdateComplaintWPF(ComplaintGridDto complaint);
    public Task ReceiveMessageWPF(MessageGridDto model);
    public Task ReceiveComment(Guid commentId, Guid offerId, string userName, string message);
    public Task ReceiveCommentWPF(CommentGridDto comment);
    public Task ReceiveQuestionWPF(QuestionAnswerGridDto model);
    public Task ReceiveQuestion(Guid announcementId, Guid questionId, string userName, string message);
    public Task ReceiveAnswer(Guid answerId, Guid questionId, string userName, string message);
    public Task ReceiveAnswerWPF(QuestionAnswerGridDto model);
    public Task UpdateChatList(Guid chatId, Guid? offerId, string userName, string message);
    public Task ReceiveOffer(AnnouncementShortDto offer);
    public Task ReceiveOfferWPF(AnnouncementGridDto offer);
    public Task UpdateOffer(AnnouncementShortDto offer);
    public Task UpdateOfferWPF(AnnouncementGridDto offer);
    public Task UpdateFullOffer(string changedById, AnnouncementFullDto offer);
    public Task DeleteFullOffer();
    public Task DeleteOfferFullQuestion();
    public Task DeleteOfferFullComment();
    public Task DeleteOffer(Guid offerId);
    public Task DeleteOfferWPF(Guid offerId);
    public Task DeleteAnswer(Guid answerId);
    public Task DeleteAnswerWPF(Guid answerId);
    public Task DeleteQuestion(Guid questionId);
    public Task DeleteComment(Guid commentId);
    public Task DeleteCommentWPF(Guid commentId);
    public Task DeleteQuestionWPF(Guid questionId);
}