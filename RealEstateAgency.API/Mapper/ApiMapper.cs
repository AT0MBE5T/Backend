using RealEstateAgency.API.Dto;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;
using Riok.Mapperly.Abstractions;

namespace RealEstateAgency.API.Mapper;

[Mapper]
public partial class ApiMapper
{
    [MapperIgnoreTarget(nameof(LoginResponseDto.Id))]
    [MapperIgnoreTarget(nameof(LoginResponseDto.AccessToken))]
    [MapperIgnoreSource(nameof(LoginRequestDto.Password))]
    public partial LoginResponseDto LoginRequestToResponse(LoginRequestDto request);
    
    [MapperIgnoreTarget(nameof(RegisterResponseDto.Id))]
    [MapperIgnoreTarget(nameof(RegisterResponseDto.AccessToken))]
    [MapperIgnoreSource(nameof(RegisterRequestDto.Age))]
    [MapperIgnoreSource(nameof(RegisterRequestDto.Name))]
    [MapperIgnoreSource(nameof(RegisterRequestDto.Surname))]
    [MapperIgnoreSource(nameof(RegisterRequestDto.Password))]
    [MapperIgnoreSource(nameof(RegisterRequestDto.Email))]
    [MapperIgnoreSource(nameof(RegisterRequestDto.PhoneNumber))]
    [MapperIgnoreSource(nameof(RegisterRequestDto.Avatar))]
    public partial RegisterResponseDto RegisterRequestToResponse(RegisterRequestDto request);
    
    [MapperIgnoreSource(nameof(AnnouncementRequest.Photos))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.PropertyType))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Location))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Area))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Floors))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Rooms))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Description))]
    [MapperIgnoreTarget(nameof(StatementDto.Id))]
    [MapProperty(nameof(AnnouncementRequest.StatementType), nameof(StatementDto.StatementTypeId))]
    public partial StatementDto AnnouncementRequestToStatementDto(
        AnnouncementRequest request, 
        Guid propertyId, 
        DateTime createdAt
    );
    
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.NewPhotos))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.DeletedImageIds))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.ExistingImageOrder))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.PropertyType))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Location))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Area))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Floors))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Rooms))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Description))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.AnnouncementId))]
    [MapperIgnoreTarget(nameof(StatementDto.Id))]
    [MapProperty(nameof(AnnouncementEditRequest.StatementType), nameof(StatementDto.StatementTypeId))]
    public partial StatementDto AnnouncementEditRequestToStatementDto(
        AnnouncementEditRequest request, 
        Guid propertyId, 
        DateTime createdAt
    );
    
    [MapperIgnoreSource(nameof(AnnouncementRequest.Photos))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Title))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Price))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.StatementType))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Content))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.UserId))]
    [MapperIgnoreTarget(nameof(PropertyDto.Id))]
    [MapProperty(nameof(AnnouncementRequest.PropertyType), nameof(PropertyDto.PropertyTypeId))]
    public partial PropertyDto AnnouncementRequestToPropertyDto(
        AnnouncementRequest request
    );
    
    [MapperIgnoreSource(nameof(AnnouncementRequest.Photos))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Title))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Price))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.StatementType))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Content))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.UserId))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.PropertyType))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Location))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Area))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Floors))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Rooms))]
    [MapperIgnoreSource(nameof(AnnouncementRequest.Description))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.Id))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.PublishedAt))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.ClosedAt))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.UpdatedAt))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.UpdatedBy))]
    public partial AnnouncementDto AnnouncementRequestToAnnouncementDto(
        AnnouncementRequest request, 
        Guid statementId,
        bool isActive
    );
    
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.NewPhotos))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.DeletedImageIds))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.ExistingImageOrder))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Title))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Price))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.StatementType))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Content))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.UserId))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.PropertyType))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Location))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Area))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Floors))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Rooms))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Description))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.AnnouncementId))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.Id))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.PublishedAt))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.ClosedAt))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.UpdatedAt))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.UpdatedBy))]
    public partial AnnouncementDto AnnouncementEditRequestToAnnouncementDto(
        AnnouncementEditRequest request, 
        Guid statementId,
        bool isActive
    );
    
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.StatementType))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.NewPhotos))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.DeletedImageIds))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.ExistingImageOrder))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Title))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Content))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.AnnouncementId))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.Price))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequest.UserId))]
    [MapperIgnoreTarget(nameof(PropertyDto.Id))]
    [MapProperty(nameof(AnnouncementEditRequest.PropertyType), nameof(PropertyDto.PropertyTypeId))]
    public partial PropertyDto AnnouncementEditRequestToPropertyDto(
        AnnouncementEditRequest request
    );
    
    [MapperIgnoreSource(nameof(AnnouncementsShortAndPages.Data))]
    [MapProperty(nameof(AnnouncementsShortAndPages.PagesCnt), nameof(AnnouncementsResponseAndPages.TotalPages))]
    [MapProperty(nameof(AnnouncementsShortAndPages.TotalItems), nameof(AnnouncementsResponseAndPages.TotalItems))]
    public partial AnnouncementsResponseAndPages ToAnnouncementsResponseAndPages(
        AnnouncementsShortAndPages source, 
        List<AnnouncementResponse> data 
    );
    
    public partial List<AnnouncementResponse> ListAnnouncementsShortAndPagesToListAnnouncementResponse(List<AnnouncementShort> announcements);

    private partial AnnouncementResponse AnnouncementsShortAndPagesToAnnouncementResponse(AnnouncementShort announcements);
    
    // private string MapPhoto(byte[] photo)
    // {
    //     return photo.Length == 0
    //                 ? string.Empty
    //                 : $"data:image/png;base64,{Convert.ToBase64String(photo)}";
    // }
    
    [MapperIgnoreTarget(nameof(PaymentDto.Id))]
    public partial PaymentDto BuyRequestToPaymentDto(BuyRequest source);
    
    [MapProperty(nameof(ReportUserRequest.DateFrom), nameof(ReportUserDto.DateFrom), Use = nameof(MapDateFrom))]
    [MapProperty(nameof(ReportUserRequest.DateTo), nameof(ReportUserDto.DateTo), Use = nameof(MapDateTo))]
    public partial ReportUserDto ReportUserRequestToReportUserDto(ReportUserRequest source);
    
    private DateTime MapDateFrom(string dateFrom)
    {
        return DateTime.Parse(dateFrom);
    }
    
    private DateTime MapDateTo(string dateTo)
    {
        return string.IsNullOrEmpty(dateTo)
            ? default
            : DateTime.Parse(dateTo);
    }
    
    [MapperIgnoreSource(nameof(CommentDto.UserId))]
    public partial CommentResponse CommentDtoToCommentResponse(CommentDto source, string author);
    
    [MapperIgnoreTarget(nameof(User.AnnouncementsNavigation))]
    [MapperIgnoreTarget(nameof(User.AnswersNavigation))]
    [MapperIgnoreTarget(nameof(User.AuHistoriesNavigation))]
    [MapperIgnoreTarget(nameof(User.CommentsNavigation))]
    [MapperIgnoreTarget(nameof(User.PaymentsNavigation))]
    [MapperIgnoreTarget(nameof(User.QuestionsNavigation))]
    [MapperIgnoreTarget(nameof(User.StatementsNavigation))]
    [MapperIgnoreTarget(nameof(User.VerificationsNavigation))]
    [MapperIgnoreTarget(nameof(User.ViewsNavigation))]
    [MapperIgnoreTarget(nameof(User.ChatMembersNavigation))]
    [MapperIgnoreTarget(nameof(User.ComplaintsNavigation))]
    [MapperIgnoreTarget(nameof(User.FavoritesNavigation))]
    [MapperIgnoreTarget(nameof(User.UserPushSubscriptionsNavigation))]
    [MapperIgnoreTarget(nameof(User.MessagesNavigation))]
    [MapperIgnoreTarget(nameof(User.PublicAvatarId))]
    [MapperIgnoreTarget(nameof(User.NormalizedEmail))]
    [MapperIgnoreTarget(nameof(User.NormalizedUserName))]
    [MapperIgnoreTarget(nameof(User.Id))]
    [MapperIgnoreTarget(nameof(User.EmailConfirmed))]
    [MapperIgnoreTarget(nameof(User.PhoneNumberConfirmed))]
    [MapperIgnoreTarget(nameof(User.PasswordHash))]
    [MapperIgnoreTarget(nameof(User.LockoutEnabled))]
    [MapperIgnoreTarget(nameof(User.LockoutEnd))]
    [MapperIgnoreTarget(nameof(User.AccessFailedCount))]
    [MapperIgnoreTarget(nameof(User.TwoFactorEnabled))]
    [MapperIgnoreTarget(nameof(User.SecurityStamp))]
    [MapperIgnoreTarget(nameof(User.ConcurrencyStamp))]
    [MapperIgnoreSource(nameof(RegisterRequestDto.Password))]
    [MapProperty(nameof(RegisterRequestDto.Login), nameof(User.UserName))]
    public partial User RegisterRequestDtoToUser(RegisterRequestDto source, DateTime createdAt);
}