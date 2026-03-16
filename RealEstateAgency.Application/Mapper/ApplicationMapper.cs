using RealEstateAgency.API.Dto;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;
using Riok.Mapperly.Abstractions;

namespace RealEstateAgency.Application.Mapper;

[Mapper]
public partial class ApplicationMapper
{
    [MapperIgnoreSource(nameof(Announcement.CommentsNavigation))]
    [MapperIgnoreSource(nameof(Announcement.QuestionsNavigation))]
    [MapperIgnoreSource(nameof(Announcement.StatementNavigation))]
    [MapperIgnoreSource(nameof(Announcement.PaymentNavigation))]
    [MapperIgnoreSource(nameof(Announcement.UserNavigation))]
    [MapperIgnoreSource(nameof(Announcement.VerificationNavigation))]
    [MapperIgnoreTarget(nameof(AnnouncementDto.IsActive))]
    public partial AnnouncementDto AnnouncementEntityToAnnouncementDto(Announcement announcementEntity);
    
    [MapperIgnoreTarget(nameof(Announcement.CommentsNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.QuestionsNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.StatementNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.PaymentNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.UserNavigation))]
    [MapperIgnoreTarget(nameof(Announcement.VerificationNavigation))]
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
    
    [MapperIgnoreSource(nameof(Comment.AnnouncementNavigation))]
    [MapperIgnoreSource(nameof(Comment.UserNavigation))]
    public partial CommentDto CommentEntityToCommentDto(Comment commentEntity);
    
    [MapperIgnoreTarget(nameof(Comment.AnnouncementNavigation))]
    [MapperIgnoreTarget(nameof(Comment.UserNavigation))]
    public partial Comment CommentDtoToCommentEntity(CommentDto commentDto);
    
    [MapperIgnoreSource(nameof(Question.AnnouncementNavigation))]
    [MapperIgnoreSource(nameof(Question.UserNavigation))]
    [MapperIgnoreSource(nameof(Question.AnswerNavigation))]
    public partial QuestionDto QuestionEntityToQuestionDto(Question questionEntity);
    
    [MapperIgnoreTarget(nameof(Question.AnnouncementNavigation))]
    [MapperIgnoreTarget(nameof(Question.UserNavigation))]
    [MapperIgnoreTarget(nameof(Question.AnswerNavigation))]
    public partial Question QuestionDtoToQuestionEntity(QuestionDto questionDto);
    
    [MapperIgnoreSource(nameof(Answer.QuestionNavigation))]
    [MapperIgnoreSource(nameof(Answer.UserNavigation))]
    public partial AnswerDto AnswerEntityToAnswerDto(Answer answerEntity);
    
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
    private partial PropertyTypeStatsDto MapClientToRequest(PropertyTypeTopClientDto clientDto);
    
    public GeneralStatsResponseDto ToGeneralStatsResponseDto(
        GeneralTopDeal topDeal,
        List<GeneralTopRealtors> topRealtors,
        List<GeneralTopProperty> topPropertyTypes,
        List<GeneralTopClient> topClients,
        int totalPlacedAnnouncements,
        decimal totalIncome)
    {
        var target = MapDealsToGeneralStatsResponseDto(topDeal);
        var realtorFirst =  MapRealtorFirstToGeneralStatsResponseDto(topRealtors.ElementAtOrDefault(0) ?? new GeneralTopRealtors());
        var realtorSecond = MapRealtorSecondToGeneralStatsResponseDto(topRealtors.ElementAtOrDefault(1) ?? new GeneralTopRealtors());
        var propertyFirst = MapPropertiesToGeneralStatsResponseFirstDto(topPropertyTypes.ElementAtOrDefault(0) ?? new GeneralTopProperty());
        var propertySecond = MapPropertiesToGeneralStatsResponseSecondDto(topPropertyTypes.ElementAtOrDefault(1) ?? new GeneralTopProperty());
        var clientFirst = MapClientsToGeneralStatsResponseFirstDto(topClients.ElementAtOrDefault(0) ?? new GeneralTopClient());
        var clientSecond = MapClientsToGeneralStatsResponseSecondDto(topClients.ElementAtOrDefault(1) ?? new GeneralTopClient());
    
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
    private partial GeneralStatsResponseDto MapDealsToGeneralStatsResponseDto(GeneralTopDeal dealDto);
    
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
    [MapProperty(nameof(GeneralTopRealtors.TopRealtorDeals), nameof(GeneralStatsResponseDto.TopRealtorDealsFirst))]
    [MapProperty(nameof(GeneralTopRealtors.TopRealtorIncome), nameof(GeneralStatsResponseDto.TopRealtorIncomeFirst))]
    [MapProperty(nameof(GeneralTopRealtors.TopRealtorName), nameof(GeneralStatsResponseDto.TopRealtorNameFirst))]
    private partial GeneralStatsResponseDto MapRealtorFirstToGeneralStatsResponseDto(GeneralTopRealtors realtorDto);
    
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
    [MapProperty(nameof(GeneralTopRealtors.TopRealtorDeals), nameof(GeneralStatsResponseDto.TopRealtorDealsSecond))]
    [MapProperty(nameof(GeneralTopRealtors.TopRealtorIncome), nameof(GeneralStatsResponseDto.TopRealtorIncomeSecond))]
    [MapProperty(nameof(GeneralTopRealtors.TopRealtorName), nameof(GeneralStatsResponseDto.TopRealtorNameSecond))]
    private partial GeneralStatsResponseDto MapRealtorSecondToGeneralStatsResponseDto(GeneralTopRealtors realtorDto);
    
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
    [MapProperty(nameof(GeneralTopProperty.TopPropertyTypeAvgPrice), nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceFirst))]
    [MapProperty(nameof(GeneralTopProperty.TopPropertyTypeCnt), nameof(GeneralStatsResponseDto.TopPropertyTypeCntFirst))]
    [MapProperty(nameof(GeneralTopProperty.TopPropertyTypeName), nameof(GeneralStatsResponseDto.TopPropertyTypeNameFirst))]
    private partial GeneralStatsResponseDto MapPropertiesToGeneralStatsResponseFirstDto(GeneralTopProperty propertyDto);
    
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
    [MapProperty(nameof(GeneralTopProperty.TopPropertyTypeAvgPrice), nameof(GeneralStatsResponseDto.TopPropertyTypeAvgPriceSecond))]
    [MapProperty(nameof(GeneralTopProperty.TopPropertyTypeCnt), nameof(GeneralStatsResponseDto.TopPropertyTypeCntSecond))]
    [MapProperty(nameof(GeneralTopProperty.TopPropertyTypeName), nameof(GeneralStatsResponseDto.TopPropertyTypeNameSecond))]
    private partial GeneralStatsResponseDto MapPropertiesToGeneralStatsResponseSecondDto(GeneralTopProperty propertyDto);
    
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
    [MapProperty(nameof(GeneralTopClient.TopClientDeals), nameof(GeneralStatsResponseDto.TopClientDealsFirst))]
    [MapProperty(nameof(GeneralTopClient.TopClientName), nameof(GeneralStatsResponseDto.TopClientNameFirst))]
    [MapProperty(nameof(GeneralTopClient.TopClientSpent), nameof(GeneralStatsResponseDto.TopClientSpentFirst))]
    private partial GeneralStatsResponseDto MapClientsToGeneralStatsResponseFirstDto(GeneralTopClient clientDto);
    
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
    [MapProperty(nameof(GeneralTopClient.TopClientDeals), nameof(GeneralStatsResponseDto.TopClientDealsSecond))]
    [MapProperty(nameof(GeneralTopClient.TopClientName), nameof(GeneralStatsResponseDto.TopClientNameSecond))]
    [MapProperty(nameof(GeneralTopClient.TopClientSpent), nameof(GeneralStatsResponseDto.TopClientSpentSecond))]
    private partial GeneralStatsResponseDto MapClientsToGeneralStatsResponseSecondDto(GeneralTopClient clientDto);
    
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
    
    [MapperIgnoreSource(nameof(Favorite.AnnouncementNavigation))]
    [MapperIgnoreSource(nameof(Favorite.UserNavigation))]
    public partial FavoriteDto MapFavoriteToDto(Favorite favorite);
    
    [MapperIgnoreTarget(nameof(Favorite.AnnouncementNavigation))]
    [MapperIgnoreTarget(nameof(Favorite.UserNavigation))]
    public partial Favorite MapFavoriteDtoToEntity(FavoriteDto favoriteDto);
    
    [MapperIgnoreSource(nameof(View.AnnouncementNavigation))]
    [MapperIgnoreSource(nameof(View.UserNavigation))]
    public partial ViewDto MapViewToDto(View view);
    
    [MapperIgnoreTarget(nameof(View.AnnouncementNavigation))]
    [MapperIgnoreTarget(nameof(View.UserNavigation))]
    public partial View MapViewDtoToEntity(ViewDto viewDto);
}