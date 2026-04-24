using RealEstateAgency.API.Dtos.Requests;
using RealEstateAgency.API.Dtos.Responses;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;
using Riok.Mapperly.Abstractions;

namespace RealEstateAgency.API.Mappers;

[Mapper]
public partial class ApiMapper
{
    [MapperIgnoreTarget(nameof(LoginResponse.Id))]
    [MapperIgnoreTarget(nameof(LoginResponse.AccessToken))]
    [MapperIgnoreSource(nameof(LoginRequest.Password))]
    public partial LoginResponse LoginRequestToResponse(LoginRequest request);
    
    [MapperIgnoreTarget(nameof(RegisterResponse.Id))]
    [MapperIgnoreTarget(nameof(RegisterResponse.AccessToken))]
    [MapperIgnoreSource(nameof(RegisterRequest.Age))]
    [MapperIgnoreSource(nameof(RegisterRequest.Name))]
    [MapperIgnoreSource(nameof(RegisterRequest.Surname))]
    [MapperIgnoreSource(nameof(RegisterRequest.Password))]
    [MapperIgnoreSource(nameof(RegisterRequest.Email))]
    [MapperIgnoreSource(nameof(RegisterRequest.PhoneNumber))]
    [MapperIgnoreSource(nameof(RegisterRequest.Avatar))]
    public partial RegisterResponse RegisterRequestToResponse(RegisterRequest request);
    
    [MapperIgnoreSource(nameof(AnnouncementsShortAndPagesDto.Data))]
    [MapProperty(nameof(AnnouncementsShortAndPagesDto.PagesCnt), nameof(AnnouncementsResponseAndPages.TotalPages))]
    [MapProperty(nameof(AnnouncementsShortAndPagesDto.TotalItems), nameof(AnnouncementsResponseAndPages.TotalItems))]
    public partial AnnouncementsResponseAndPages ToAnnouncementsResponseAndPages(
        AnnouncementsShortAndPagesDto source, 
        List<AnnouncementResponse> data 
    );
    
    public partial List<AnnouncementResponse> ListAnnouncementsShortAndPagesToListAnnouncementResponse(List<AnnouncementShortDto> announcements);

    private partial AnnouncementResponse AnnouncementsShortAndPagesToAnnouncementResponse(AnnouncementShortDto announcements);
    
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
    [MapProperty(nameof(CommentDto.AuthorName), nameof(CommentResponse.Author))]
    public partial CommentResponse CommentDtoToCommentResponse(CommentDto source);
}