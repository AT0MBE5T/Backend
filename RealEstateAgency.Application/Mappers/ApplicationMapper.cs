using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace RealEstateAgency.Application.Mappers;

[Mapper]
public partial class ApplicationMapper
{
    [MapperIgnoreTarget(nameof(Announcement.CommentsNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.QuestionsNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.StatementNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.PaymentNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.UserNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.VerificationNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.ViewsNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.ComplaintsNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.FavoritesNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.ChatsNavigation))]
    [MapperIgnoreSource(nameof(AnnouncementDto.IsActive))]
    public partial Announcement AnnouncementDtoToAnnouncementEntity(AnnouncementDto announcementDto);
    
    
    [MapperIgnoreSource(nameof(Property.ImagesNavigation))]
    [MapperIgnoreSource(nameof(Property.PropertyTypeNavigation))]
    [MapperIgnoreSource(nameof(Property.StatementNavigation))]
    public partial PropertyDto PropertyEntityToPropertyDto(Property propertyEntity);
    
    [MapperIgnoreTarget(nameof(Property.ImagesNavigation))]
    [MapperIgnoreTarget(nameof(Property.PropertyTypeNavigation))]
    [MapperIgnoreTarget(nameof(Property.StatementNavigation))]
    public partial Property PropertyDtoToPropertyEntity(PropertyDto propertyDto);
    
    [MapperIgnoreSource(nameof(Statement.AnnouncementNavigation))]
    [MapperIgnoreSource(nameof(Statement.PropertyNavigation))]
    [MapperIgnoreSource(nameof(Statement.StatementTypeNavigation))]
    [MapperIgnoreSource(nameof(Statement.UserNavigation))]
    public partial StatementDto StatementEntityToStatementDto(Statement statementEntity);
    
    [MapperIgnoreTarget(nameof(Statement.AnnouncementNavigation))]
    [MapperIgnoreTarget(nameof(Statement.PropertyNavigation))]
    [MapperIgnoreTarget(nameof(Statement.StatementTypeNavigation))]
    [MapperIgnoreTarget(nameof(Statement.UserNavigation))]
    public partial Statement StatementDtoToStatementEntity(StatementDto statementEntityDto);
    
    [MapperIgnoreTarget(nameof(Comment.AnnouncementNavigation))]
    [MapperIgnoreTarget(nameof(Comment.UserNavigation))]
    [MapperIgnoreSource(nameof(CommentDto.AuthorName))]
    public partial Comment CommentDtoToCommentEntity(CommentDto commentDto);
    
    [MapperIgnoreTarget(nameof(Question.AnnouncementNavigation))]
    [MapperIgnoreTarget(nameof(Question.UserNavigation))]
    [MapperIgnoreTarget(nameof(Question.AnswerNavigation))]
    public partial Question QuestionDtoToQuestionEntity(QuestionDto questionDto);
    
    [MapperIgnoreTarget(nameof(Answer.QuestionNavigation))]
    [MapperIgnoreTarget(nameof(Answer.UserNavigation))]
    public partial Answer AnswerDtoToAnswerEntity(AnswerDto answerDto);
    
    [MapperIgnoreSource(nameof(StatementType.StatementsNavigation))]
    public partial StatementTypeDto StatementTypeEntityToStatementTypeDto(StatementType statementEntity);
    
    [MapperIgnoreSource(nameof(PropertyType.PropertiesNavigation))]
    public partial PropertyTypeDto PropertyTypeEntityToPropertyTypeDto(PropertyType propertyEntity);
    
    [MapperIgnoreTarget(nameof(AuHistory.ActionNavigation))]
    [MapperIgnoreTarget(nameof(AuHistory.UserNavigation))]
    public partial AuHistory AuditDtoToAuHistory(AuditDto auHistory);
    
    [MapperIgnoreTarget(nameof(Payment.AnnouncementNavigation))]
    [MapperIgnoreTarget(nameof(Payment.CustomerNavigation))]
    public partial Payment PaymentDtoToPaymentEntity(PaymentDto paymentDto);
    
    public AnnouncementGetEditRequest ToAnnouncementGetEditRequest(
        PropertyDto propertyDto, 
        StatementDto statementDto, 
        List<PhotoDto> photos)
    {
        var target = MapPropertyToRequest(propertyDto);
        
        target.Price = statementDto.Price;
        target.Title = statementDto.Title;
        target.Content = statementDto.Content;
        target.StatementTypeId = statementDto.StatementTypeId;
        target.UserId = statementDto.UserId;
        target.Photos = photos;
        
        return target;
    }
    
    [MapperIgnoreSource(nameof(PropertyDto.Id))]
    [MapperIgnoreTarget(nameof(AnnouncementGetEditRequest.Price))]
    [MapperIgnoreTarget(nameof(AnnouncementGetEditRequest.Title))]
    [MapperIgnoreTarget(nameof(AnnouncementGetEditRequest.Content))]
    [MapperIgnoreTarget(nameof(AnnouncementGetEditRequest.StatementTypeId))]
    [MapperIgnoreTarget(nameof(AnnouncementGetEditRequest.UserId))]
    [MapperIgnoreTarget(nameof(AnnouncementGetEditRequest.Photos))]
    private partial AnnouncementGetEditRequest MapPropertyToRequest(PropertyDto propertyDto);
    
    public PropertyTypeStatsDto ToPropertyTypeStatsDto(
        PropertyTypeTotalsDto totals, 
        PropertyTypeTopDealDto deals,
        PropertyTypeTopRealtorDto realtors,
        PropertyTypeTopClientDto clients)
    {
        var target = MapTotalsToRequest(totals);
        var targetDeals = MapDealToRequest(deals);
        var targetRealtors = MapRealtorToRequest(realtors);
        var targetClients = MapClientToRequest(clients);
    
        target.TopDealName =  targetDeals.TopDealName;
        target.TopDealStatementType =  targetDeals.TopDealStatementType;
        target.TopDealPrice =  targetDeals.TopDealPrice;
        target.TopDealRealtorName =  targetDeals.TopDealRealtorName;
        target.TopDealCustomerName =  targetDeals.TopDealCustomerName;
        target.TopDealSoldDate =  targetDeals.TopDealSoldDate;
        target.TopRealtorName = targetRealtors.TopRealtorName;
        target.TopRealtorDeals = targetRealtors.TopRealtorDeals;
        target.TopRealtorIncome = targetRealtors.TopRealtorIncome;
        target.TopClientName = targetClients.TopClientName;
        target.TopClientDeals = targetClients.TopClientDeals;
        target.TopClientSpent = targetClients.TopClientSpent;
        
        return target;
    }
    
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopRealtorName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopRealtorDeals))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopRealtorIncome))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopClientName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopClientDeals))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopClientSpent))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealStatementType))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealSoldDate))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealCustomerName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealPrice))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealRealtorName))]
    private partial PropertyTypeStatsDto MapTotalsToRequest(PropertyTypeTotalsDto totalDto);
    
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopRealtorName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopRealtorDeals))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopRealtorIncome))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopClientName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopClientDeals))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopClientSpent))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TotalPlacedAnnouncements))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TotalDeals))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TotalIncome))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.Views))]
    private partial PropertyTypeStatsDto MapDealToRequest(PropertyTypeTopDealDto dealDto);
    
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopClientName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopClientDeals))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopClientSpent))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TotalPlacedAnnouncements))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TotalDeals))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TotalIncome))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealStatementType))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealSoldDate))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealCustomerName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealPrice))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealRealtorName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.Views))]
    private partial PropertyTypeStatsDto MapRealtorToRequest(PropertyTypeTopRealtorDto realtorDto);
    
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopRealtorName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopRealtorDeals))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopRealtorIncome))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TotalPlacedAnnouncements))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TotalDeals))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TotalIncome))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealStatementType))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealSoldDate))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealCustomerName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealPrice))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.TopDealRealtorName))]
    [MapperIgnoreTarget(nameof(PropertyTypeStatsDto.Views))]
    private partial PropertyTypeStatsDto MapClientToRequest(PropertyTypeTopClientDto clientDto);
    
    public GeneralStatsResponseDto ToGeneralStatsResponseDto(
        int views,
        GeneralTopDealDto topDealDto,
        List<GeneralTopRealtorsDto> topRealtors,
        List<GeneralTopPropertyDto> topPropertyTypes,
        List<GeneralTopClientDto> topClients,
        int totalPlacedAnnouncements,
        decimal totalIncome)
    {
        var target = MapDealsToGeneralStatsResponseDto(topDealDto);
        var realtorFirst =  MapRealtorFirstToGeneralStatsResponseDto(topRealtors.ElementAtOrDefault(0) ?? new GeneralTopRealtorsDto());
        var realtorSecond = MapRealtorSecondToGeneralStatsResponseDto(topRealtors.ElementAtOrDefault(1) ?? new GeneralTopRealtorsDto());
        var propertyFirst = MapPropertiesToGeneralStatsResponseFirstDto(topPropertyTypes.ElementAtOrDefault(0) ?? new GeneralTopPropertyDto());
        var propertySecond = MapPropertiesToGeneralStatsResponseSecondDto(topPropertyTypes.ElementAtOrDefault(1) ?? new GeneralTopPropertyDto());
        var clientFirst = MapClientsToGeneralStatsResponseFirstDto(topClients.ElementAtOrDefault(0) ?? new GeneralTopClientDto());
        var clientSecond = MapClientsToGeneralStatsResponseSecondDto(topClients.ElementAtOrDefault(1) ?? new GeneralTopClientDto());

        target.Views = views;
        target.TotalClosedAnnouncements = totalPlacedAnnouncements;
        target.TotalIncome = totalIncome;
        target.TopRealtorDealsFirst = realtorFirst.TopRealtorDealsFirst;
        target.TopRealtorIncomeFirst = realtorFirst.TopRealtorIncomeFirst;
        target.TopRealtorNameFirst = realtorFirst.TopRealtorNameFirst;
        target.TopRealtorDealsSecond = realtorSecond.TopRealtorDealsSecond;
        target.TopRealtorIncomeSecond = realtorSecond.TopRealtorIncomeSecond;
        target.TopRealtorNameSecond = realtorSecond.TopRealtorNameSecond;
        target.TopPropertyTypeAvgPriceFirst = propertyFirst.TopPropertyTypeAvgPriceFirst;
        target.TopPropertyTypeCntFirst = propertyFirst.TopPropertyTypeCntFirst;
        target.TopPropertyTypeNameFirst = propertyFirst.TopPropertyTypeNameFirst;
        target.TopPropertyTypeAvgPriceSecond = propertySecond.TopPropertyTypeAvgPriceSecond;
        target.TopPropertyTypeCntSecond = propertySecond.TopPropertyTypeCntSecond;
        target.TopPropertyTypeNameSecond = propertySecond.TopPropertyTypeNameSecond;
        target.TopClientDealsFirst = clientFirst.TopClientDealsFirst;
        target.TopClientDealsSecond = clientSecond.TopClientDealsSecond;
        target.TopClientSpentFirst = clientFirst.TopClientSpentFirst;
        target.TopClientSpentSecond = clientSecond.TopClientSpentSecond;
        target.TopClientNameFirst = clientFirst.TopClientNameFirst;
        target.TopClientNameSecond = clientSecond.TopClientNameSecond;
        
        return target;
    }
    
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientDealsFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientDealsSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientSpentFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientSpentSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorDealsFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorDealsSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorIncomeFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorIncomeSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeCntFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeCntSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalClosedAnnouncements))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalIncome))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.Views))]
    private partial GeneralStatsResponseDto MapDealsToGeneralStatsResponseDto(GeneralTopDealDto dealDtoDto);
    
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientDealsFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientDealsSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientSpentFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientSpentSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorDealsSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorIncomeSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeCntFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeCntSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalClosedAnnouncements))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalIncome))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealRealtorName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealCustomerName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealPrice))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealSoldDate))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealStatementType))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.Views))]
    [MapProperty(nameof(GeneralTopRealtorsDto.TopRealtorDeals), nameof(GeneralStatsResponseDto.TopRealtorDealsFirst))]
    [MapProperty(nameof(GeneralTopRealtorsDto.TopRealtorIncome), nameof(GeneralStatsResponseDto.TopRealtorIncomeFirst))]
    [MapProperty(nameof(GeneralTopRealtorsDto.TopRealtorName), nameof(GeneralStatsResponseDto.TopRealtorNameFirst))]
    private partial GeneralStatsResponseDto MapRealtorFirstToGeneralStatsResponseDto(GeneralTopRealtorsDto realtorDtoDto);
    
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientDealsFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientDealsSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientSpentFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientSpentSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorDealsFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorIncomeFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeCntFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeCntSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalClosedAnnouncements))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalIncome))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealRealtorName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealCustomerName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealPrice))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealSoldDate))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealStatementType))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.Views))]
    [MapProperty(nameof(GeneralTopRealtorsDto.TopRealtorDeals), nameof(GeneralStatsResponseDto.TopRealtorDealsSecond))]
    [MapProperty(nameof(GeneralTopRealtorsDto.TopRealtorIncome), nameof(GeneralStatsResponseDto.TopRealtorIncomeSecond))]
    [MapProperty(nameof(GeneralTopRealtorsDto.TopRealtorName), nameof(GeneralStatsResponseDto.TopRealtorNameSecond))]
    private partial GeneralStatsResponseDto MapRealtorSecondToGeneralStatsResponseDto(GeneralTopRealtorsDto realtorDtoDto);
    
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientDealsFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientDealsSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientSpentFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientSpentSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorDealsFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorIncomeFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorDealsSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorIncomeSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeCntSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalClosedAnnouncements))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalIncome))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealRealtorName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealCustomerName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealPrice))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealSoldDate))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealStatementType))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.Views))]
    [MapProperty(nameof(GeneralTopPropertyDto.TopPropertyTypeAvgPrice), nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceFirst))]
    [MapProperty(nameof(GeneralTopPropertyDto.TopPropertyTypeCnt), nameof(GeneralStatsResponseDto.TopPropertyTypeCntFirst))]
    [MapProperty(nameof(GeneralTopPropertyDto.TopPropertyTypeName), nameof(GeneralStatsResponseDto.TopPropertyTypeNameFirst))]
    private partial GeneralStatsResponseDto MapPropertiesToGeneralStatsResponseFirstDto(GeneralTopPropertyDto propertyDtoDto);
    
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientDealsFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientDealsSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientSpentFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientSpentSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorDealsFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorIncomeFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorDealsSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorIncomeSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeCntFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalClosedAnnouncements))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalIncome))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealRealtorName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealCustomerName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealPrice))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealSoldDate))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealStatementType))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.Views))]
    [MapProperty(nameof(GeneralTopPropertyDto.TopPropertyTypeAvgPrice), nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceSecond))]
    [MapProperty(nameof(GeneralTopPropertyDto.TopPropertyTypeCnt), nameof(GeneralStatsResponseDto.TopPropertyTypeCntSecond))]
    [MapProperty(nameof(GeneralTopPropertyDto.TopPropertyTypeName), nameof(GeneralStatsResponseDto.TopPropertyTypeNameSecond))]
    private partial GeneralStatsResponseDto MapPropertiesToGeneralStatsResponseSecondDto(GeneralTopPropertyDto propertyDtoDto);
    
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientDealsSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientSpentSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorDealsFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorIncomeFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorDealsSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorIncomeSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeCntFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeCntSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalClosedAnnouncements))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalIncome))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealRealtorName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealCustomerName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealPrice))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealSoldDate))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealStatementType))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.Views))]
    [MapProperty(nameof(GeneralTopClientDto.TopClientDeals), nameof(GeneralStatsResponseDto.TopClientDealsFirst))]
    [MapProperty(nameof(GeneralTopClientDto.TopClientName), nameof(GeneralStatsResponseDto.TopClientNameFirst))]
    [MapProperty(nameof(GeneralTopClientDto.TopClientSpent), nameof(GeneralStatsResponseDto.TopClientSpentFirst))]
    private partial GeneralStatsResponseDto MapClientsToGeneralStatsResponseFirstDto(GeneralTopClientDto clientDtoDto);
    
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientDealsFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopClientSpentFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorDealsFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorIncomeFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorDealsSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorIncomeSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopRealtorNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeCntFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeNameFirst))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeCntSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopPropertyTypeNameSecond))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalClosedAnnouncements))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TotalIncome))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealRealtorName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealCustomerName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealName))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealPrice))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealSoldDate))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.TopDealStatementType))]
    [MapperIgnoreTarget(nameof(GeneralStatsResponseDto.Views))]
    [MapProperty(nameof(GeneralTopClientDto.TopClientDeals), nameof(GeneralStatsResponseDto.TopClientDealsSecond))]
    [MapProperty(nameof(GeneralTopClientDto.TopClientName), nameof(GeneralStatsResponseDto.TopClientNameSecond))]
    [MapProperty(nameof(GeneralTopClientDto.TopClientSpent), nameof(GeneralStatsResponseDto.TopClientSpentSecond))]
    private partial GeneralStatsResponseDto MapClientsToGeneralStatsResponseSecondDto(GeneralTopClientDto clientDtoDto);
    
    [MapperIgnoreSource(nameof(User.AnnouncementsNavigation))]
    [MapperIgnoreSource(nameof(User.AnswersNavigation))]
    [MapperIgnoreSource(nameof(User.AuHistoriesNavigation))]
    [MapperIgnoreSource(nameof(User.CommentsNavigation))]
    [MapperIgnoreSource(nameof(User.PaymentsNavigation))]
    [MapperIgnoreSource(nameof(User.QuestionsNavigation))]
    [MapperIgnoreSource(nameof(User.StatementsNavigation))]
    [MapperIgnoreSource(nameof(User.VerificationsNavigation))]
    [MapperIgnoreSource(nameof(User.Id))]
    [MapperIgnoreSource(nameof(User.NormalizedEmail))]
    [MapperIgnoreSource(nameof(User.NormalizedUserName))]
    [MapperIgnoreSource(nameof(User.EmailConfirmed))]
    [MapperIgnoreSource(nameof(User.PhoneNumberConfirmed))]
    [MapperIgnoreSource(nameof(User.LockoutEnabled))]
    [MapperIgnoreSource(nameof(User.LockoutEnd))]
    [MapperIgnoreSource(nameof(User.AccessFailedCount))]
    [MapperIgnoreSource(nameof(User.PasswordHash))]
    [MapperIgnoreSource(nameof(User.SecurityStamp))]
    [MapperIgnoreSource(nameof(User.ConcurrencyStamp))]
    [MapperIgnoreSource(nameof(User.TwoFactorEnabled))]
    
    [MapperIgnoreSource(nameof(User.ViewsNavigation))]
    [MapperIgnoreSource(nameof(User.ComplaintsNavigation))]
    [MapperIgnoreSource(nameof(User.FavoritesNavigation))]
    [MapperIgnoreSource(nameof(User.AnswersNavigation))]
    [MapperIgnoreSource(nameof(User.AnnouncementsNavigation))]
    [MapperIgnoreSource(nameof(User.AuHistoriesNavigation))]
    [MapperIgnoreSource(nameof(User.ChatMembersNavigation))]
    [MapperIgnoreSource(nameof(User.CommentsNavigation))]
    [MapperIgnoreSource(nameof(User.MessagesNavigation))]
    [MapperIgnoreSource(nameof(User.PaymentsNavigation))]
    [MapperIgnoreSource(nameof(User.QuestionsNavigation))]
    [MapperIgnoreSource(nameof(User.StatementsNavigation))]
    [MapperIgnoreSource(nameof(User.VerificationsNavigation))]
    [MapperIgnoreSource(nameof(User.UserPushSubscriptionsNavigation))]
    
    [MapperIgnoreTarget(nameof(UserDto.Roles))]
    
    [MapProperty(nameof(User.UserName), nameof(UserDto.Login))]
    [MapProperty(nameof(User.Avatar), nameof(UserDto.AvatarUrl))]
    [MapProperty(nameof(User.PublicAvatarId), nameof(UserDto.PublicId))]
    public partial UserDto UserToUserDto(User user);
    
    [MapperIgnoreSource(nameof(Complaint.AdminNavigation))]
    [MapperIgnoreSource(nameof(Complaint.AnnouncementNavigation))]
    [MapperIgnoreSource(nameof(Complaint.ComplaintStatusNavigation))]
    [MapperIgnoreSource(nameof(Complaint.ComplaintTypeNavigation))]
    [MapperIgnoreSource(nameof(Complaint.UserNavigation))]
    public partial ComplaintDto MapComplaintToDto(Complaint complaint);
    
    [MapperIgnoreTarget(nameof(Complaint.AdminNavigation))]
    [MapperIgnoreTarget(nameof(Complaint.AnnouncementNavigation))]
    [MapperIgnoreTarget(nameof(Complaint.ComplaintStatusNavigation))]
    [MapperIgnoreTarget(nameof(Complaint.ComplaintTypeNavigation))]
    [MapperIgnoreTarget(nameof(Complaint.UserNavigation))]
    public partial Complaint MapComplaintDtoToEntity(ComplaintDto complaintDto);
    
    [MapperIgnoreTarget(nameof(Favorite.AnnouncementNavigation))]
    [MapperIgnoreTarget(nameof(Favorite.UserNavigation))]
    public partial Favorite MapFavoriteDtoToEntity(FavoriteDto favoriteDto);
    
    [MapperIgnoreTarget(nameof(View.AnnouncementNavigation))]
    [MapperIgnoreTarget(nameof(View.UserNavigation))]
    public partial View MapViewDtoToEntity(ViewDto viewDto);
    
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Photos))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Title))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Price))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.StatementType))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Content))]
    [MapperIgnoreTarget(nameof(PropertyDto.Id))]
    [MapProperty(nameof(AnnouncementRequestAppDto.PropertyType), nameof(PropertyDto.PropertyTypeId))]
    public partial PropertyDto AnnouncementRequestToPropertyDto(
        AnnouncementRequestAppDto requestAppDto
    );
    
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Photos))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Title))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Price))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.StatementType))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Content))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.PropertyType))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Location))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Area))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Floors))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Rooms))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Description))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.Id))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.PublishedAt))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.ClosedAt))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.UpdatedAt))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.UpdatedBy))]
    public partial AnnouncementDto AnnouncementRequestToAnnouncementDto(
        AnnouncementRequestAppDto requestAppDto, 
        Guid statementId,
        bool isActive
    );
    
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Photos))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.PropertyType))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Location))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Area))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Floors))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Rooms))]
    [MapperIgnoreSource(nameof(AnnouncementRequestAppDto.Description))]
    [MapperIgnoreTarget(nameof(StatementDto.UserId))]
    [MapperIgnoreTarget(nameof(StatementDto.Id))]
    [MapProperty(nameof(AnnouncementRequestAppDto.StatementType), nameof(StatementDto.StatementTypeId))]
    public partial StatementDto AnnouncementRequestToStatementDto(
        AnnouncementRequestAppDto requestAppDto, 
        Guid propertyId, 
        DateTime createdAt
    );
    
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.NewPhotos))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.DeletedImageIds))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.ExistingImageOrder))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.PropertyType))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Location))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Area))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Floors))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Rooms))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Description))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.AnnouncementId))]
    [MapperIgnoreTarget(nameof(StatementDto.Id))]
    [MapProperty(nameof(AnnouncementEditRequestAppDto.StatementType), nameof(StatementDto.StatementTypeId))]
    public partial StatementDto AnnouncementEditRequestToStatementDto(
        AnnouncementEditRequestAppDto requestAppDto, 
        Guid propertyId, 
        DateTime createdAt
    );
    
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.NewPhotos))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.DeletedImageIds))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.ExistingImageOrder))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Title))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Price))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.StatementType))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Content))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.UserId))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.PropertyType))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Location))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Area))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Floors))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Rooms))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Description))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.AnnouncementId))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.Id))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.PublishedAt))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.ClosedAt))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.UpdatedAt))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.UpdatedBy))]
    public partial AnnouncementDto AnnouncementEditRequestToAnnouncementDto(
        AnnouncementEditRequestAppDto requestAppDto, 
        Guid statementId,
        bool isActive
    );
    
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.StatementType))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.NewPhotos))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.DeletedImageIds))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.ExistingImageOrder))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Title))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Content))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.AnnouncementId))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.Price))]
    [MapperIgnoreSource(nameof(AnnouncementEditRequestAppDto.UserId))]
    [MapperIgnoreTarget(nameof(PropertyDto.Id))]
    [MapProperty(nameof(AnnouncementEditRequestAppDto.PropertyType), nameof(PropertyDto.PropertyTypeId))]
    public partial PropertyDto AnnouncementEditRequestToPropertyDto(
        AnnouncementEditRequestAppDto requestAppDto
    );
    
    [MapperIgnoreTarget(nameof(PaymentDto.Id))]
    public partial PaymentDto CloseAnnouncementToPaymentDto(CloseAnnouncementCommandDto source);
}