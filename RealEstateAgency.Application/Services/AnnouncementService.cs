using System.Net;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;
using ApplicationMapper = RealEstateAgency.Application.Mappers.ApplicationMapper;

namespace RealEstateAgency.Application.Services;

public class AnnouncementService(IAnnouncementRepository announcementRepository, IStatementService statementService,
    IAuditService auditService, IPropertyService propertyService, IImageService imageService,
    ApplicationMapper mapper, IVerificationRepository verificationRepository, IUnitOfWork unitOfWork,
    IHubService hubService, IPaymentService paymentService) : IAnnouncementsService
{
    public async Task<AnnouncementGetEditRequest?> GetAnnouncementForEditByIdAsync(Guid announcementId)
    {
        var announcement = await announcementRepository.GetAnnouncementById(announcementId);
        if (announcement == null)
        {
            return null;
        }

        var statement = await statementService.GetStatementByIdAsync(announcement.StatementId);
        if (statement == null)
        {
            return null;
        }
        
        var property = await propertyService.GetPropertyByIdAsync(statement.PropertyId);
        if (property == null)
        {
            return null;
        }

        var photos = await imageService.GetPhotosByPropertyIdAsync(statement.PropertyId);
        var res = mapper.ToAnnouncementGetEditRequest(property, statement, photos);
        
        return res;
    }

    private async Task Verify(SwitchVerificationCommandDto commandDto)
    {
        var data = new Verification
        {
            Id = Guid.NewGuid(),
            CreatedBy = commandDto.UserId,
            CreatedAt = DateTime.UtcNow,
            AnnouncementId = commandDto.AnnouncementId,
        };

        await verificationRepository.Insert(data);
        var auditDto = new AuditDto
        {
            ActionId = Guid.Parse(AuditAction.Verify),
            UserId = commandDto.UserId,
            Details = $"Announcement {commandDto.AnnouncementId} verified by {commandDto.UserId}"
        };
                
        await auditService.InsertAudit(auditDto);
    }
    
    private async Task Unverify(SwitchVerificationCommandDto commandDto, Verification verification)
    {
        await verificationRepository.Delete(verification);
        var auditDto = new AuditDto
        {
            ActionId = Guid.Parse(AuditAction.Unverify),
            UserId = commandDto.UserId,
            Details = $"Announcement {commandDto.AnnouncementId} unverified by {commandDto.UserId}"
        };
                
        await auditService.InsertAudit(auditDto);
    }

    public async Task<string> SwitchVerifyAnnouncement(SwitchVerificationCommandDto commandDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var isPaid = await paymentService.IsExistByAnnouncementIdAsync(commandDto.AnnouncementId);

            if (isPaid)
                return "Already paid";
            
            var verification = await announcementRepository.GetVerificationAsync(commandDto.AnnouncementId);
            if (verification == null)
            {
                await Verify(commandDto);
            }
            else
            {
                await Unverify(commandDto, verification);
            }

            await unitOfWork.CommitAsync();
            return string.Empty;
        }
        catch(Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return ex.Message;
        }
    }
    
    public async Task<CloseAnnouncementResponseDto> CloseAnnouncement(CloseAnnouncementCommandDto commandDto)
    {
        var isExist = await paymentService.IsExistByAnnouncementIdAsync(commandDto.AnnouncementId);

        if (isExist)
            return new CloseAnnouncementResponseDto(Guid.Empty, "Payment is exist");
        
        var paymentDto = mapper.CloseAnnouncementToPaymentDto(commandDto);
        
        if (!await SetClosedAt(paymentDto.AnnouncementId))
        {
            return new CloseAnnouncementResponseDto(Guid.Empty, "SetClosed error");
        }
        
        var paymentId = await paymentService.InsertPayment(paymentDto);
        return new CloseAnnouncementResponseDto(paymentId ?? Guid.Empty, string.Empty);
    }

    private async Task<Guid?> AddAnnouncement(Guid userId, AnnouncementDto announcementDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var entity = mapper.AnnouncementDtoToAnnouncementEntity(announcementDto);
            await announcementRepository.InsertAsync(entity);
            
            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.CreateAnnouncement),
                UserId = userId,
                Details = $"Announcement {entity.Id} created by {userId}"
            };
            
            await auditService.InsertAudit(auditDto);
            await unitOfWork.CommitAsync();
            
            return announcementDto.Id;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            return null;
        }
    }
    
    public async Task<AddAnnouncementResponseDto> CreateAnnouncementAsync(CreateAnnouncementCommandDto commandDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var announcementRequest = new AnnouncementRequestAppDto
            {
                Area = commandDto.Area,
                Content = commandDto.Content,
                Description = commandDto.Description,
                Floors = commandDto.Floors,
                Location = commandDto.Location,
                Photos = commandDto.Photos,
                Price = commandDto.Price,
                PropertyType = commandDto.PropertyType,
                Rooms = commandDto.Rooms,
                StatementType = commandDto.StatementType,
                Title = commandDto.Title
            };

            var propertyDto = mapper.AnnouncementRequestToPropertyDto(announcementRequest);
            var propertyId = await propertyService.AddProperty(propertyDto);
            
            for (var i = 0; i < commandDto.Photos.Count; i++)
            {
                var uploadResult = await imageService.UploadImageAsync(
                    commandDto.Photos[i].Content,
                    commandDto.Photos[i].FileName);

                await imageService.InsertAsync(new ImageDto
                {
                    PhotoUrl = uploadResult.Url,
                    PublicId = uploadResult.PublicId,
                    PropertyId = propertyId,
                    OrderIndex = i
                });
            }
            
            var statement = mapper.AnnouncementRequestToStatementDto(announcementRequest, propertyId, DateTime.UtcNow);
            var statementId = await statementService.AddStatementAsync(statement);
            
            if (statementId is null)
                return new AddAnnouncementResponseDto((int)HttpStatusCode.BadRequest, "Statement not created", Guid.Empty);
            
            var announcement = mapper.AnnouncementRequestToAnnouncementDto(announcementRequest, (Guid)statementId, true);
            var announcementId = await AddAnnouncement(commandDto.UserId, announcement);

            if (announcementId is null)
                return new AddAnnouncementResponseDto((int)HttpStatusCode.BadRequest, "Announcement not created", Guid.Empty);
            
            var offerDto = await announcementRepository.GetAnnouncementShortByOfferId((Guid)announcementId, commandDto.UserId);
            await hubService.NotifyNewOfferAsync(offerDto);

            return new AddAnnouncementResponseDto((int)HttpStatusCode.Created, string.Empty, (Guid)announcementId);
        }
        catch(Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new AddAnnouncementResponseDto((int)HttpStatusCode.BadRequest, ex.Message, Guid.Empty);
        }
    }
    
    private async Task<bool> UpdateAsync(Guid announcementId, AnnouncementDto announcementDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var announcement = mapper.AnnouncementDtoToAnnouncementEntity(announcementDto);
            var isUpdated = await announcementRepository.UpdateAsync(announcementId, announcement);
            if (!isUpdated)
            {
                await unitOfWork.RollbackAsync();
                return false;
            }
    
            await unitOfWork.CommitAsync();
            return true;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            return false;
        }
    }
    
    public async Task<string> UpdateAnnouncementAsync(AnnouncementUpdateCommandDto commandDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var request = new AnnouncementEditRequestAppDto
            {
                Area = commandDto.Area,
                Content = commandDto.Content,
                Description = commandDto.Description,
                Floors = commandDto.Floors,
                Location = commandDto.Location,
                UserId = commandDto.UserId,
                AnnouncementId = commandDto.AnnouncementId,
                DeletedImageIds = commandDto.DeletedImageIds,
                ExistingImageOrder = commandDto.ExistingImageOrder,
                NewPhotos = commandDto.NewPhotos,
                Price = commandDto.Price,
                PropertyType = commandDto.PropertyType,
                Rooms = commandDto.Rooms,
                StatementType = commandDto.StatementType,
                Title = commandDto.Title
            };
            
            var statementId = await GetStatementIdByAnnouncementIdAsync(commandDto.AnnouncementId);
            var propertyId = await GetPropertyIdByAnnouncementIdAsync(commandDto.AnnouncementId);

            if (statementId == null || propertyId == null)
            {
                return "Statement or property not found";
            }
            
            var photos = commandDto.NewPhotos?.ToList() ??  [];
            var deleteImages = commandDto.DeletedImageIds ?? [];
            var existingImageOrder = commandDto.ExistingImageOrder ?? [];

            foreach (var id in deleteImages)
            {
                await imageService.DeleteAsync(id);
            }

            var newPhotoIndex = 0;
            var orderIndex = 0;

            foreach (var item in existingImageOrder)
            {
                if (item == "new")
                {
                    var file = photos[newPhotoIndex];
                    newPhotoIndex++;

                    if (file.Content.Length == 0)
                        return "Image is required";
                    
                    var imageResult = await imageService.UploadImageAsync(file.Content, file.FileName);

                    if (imageResult.Error != null)
                        throw new Exception(imageResult.Error);

                    await imageService.InsertAsync(new ImageDto
                    {
                        PhotoUrl = imageResult.Url,
                        PublicId = imageResult.PublicId,
                        PropertyId = propertyId,
                        OrderIndex = orderIndex
                    });
                }
                else
                {
                    await imageService.UpdateOrderAsync(Guid.Parse(item), orderIndex);
                }

                orderIndex++;
            }

            var property = mapper.AnnouncementEditRequestToPropertyDto(request);
            var propertyRes = await propertyService.UpdatePropertyAsync((Guid)propertyId, property);
            
            if (!propertyRes)
            {
                return "Property not updated";
            }
            
            var statement = mapper.AnnouncementEditRequestToStatementDto(request, (Guid)propertyId, DateTime.UtcNow);
            var statementRes = await statementService.UpdateStatementAsync((Guid)statementId, commandDto.UserId, statement);
            
            if (!statementRes)
            {
                return "Statement not updated";
            }
            
            var announcement = mapper.AnnouncementEditRequestToAnnouncementDto(request, (Guid)statementId, true);
            var response = await UpdateAsync(commandDto.AnnouncementId, announcement);
            
            var offerDto = await GetAnnouncementShortByOfferId(commandDto.AnnouncementId, commandDto.UserId);

            await hubService.NotifyUpdateOfferAsync(offerDto);

            return string.Empty;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return ex.Message;
        }
    }

    private async Task<bool> DeleteAsync(Guid announcementId, Guid userId)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var announcement = await announcementRepository.GetAnnouncementById(announcementId);
            if (announcement == null)
            {
                await unitOfWork.RollbackAsync();
                return false;
            }

            if (await announcementRepository.DeleteAsync(announcementId))
            {
                var auditDto = new AuditDto
                {
                    ActionId = Guid.Parse(AuditAction.DeleteAnnouncement),
                    UserId = userId,
                    Details = $"Announcement {announcementId} deleted by {userId}"
                };
                
                await auditService.InsertAudit(auditDto);

                await unitOfWork.CommitAsync();
                return true;
            }
            
            await unitOfWork.RollbackAsync();
            return false;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
        }

        return false;
    }
        
    public async Task<string> DeleteAnnouncementAsync(DeleteAnnouncementCommandDto commandDto)
    {
        var isPaid = await paymentService.IsExistByAnnouncementIdAsync(commandDto.AnnouncementId);

        if (isPaid)
            return "Announcement already paid";

        var result = await DeleteAsync(commandDto.AnnouncementId, commandDto.UserId);

        await hubService.DeleteOfferAsync(commandDto.AnnouncementId);

        return string.Empty;
    }

    public async Task<Guid> GetAuthorOfferIdByQuestionId(Guid questionId)
    {
        var data = await announcementRepository.GetAuthorOfferIdByQuestionIdAsync(questionId);
        return data;
    }
    
    public async Task<AnnouncementsShortAndPagesDto> GetSearchDataPaginated(string text, List<string> filters, int sortId, int page, int limit, Guid userId)
    {
        var data = await announcementRepository.GetSearchData(text, filters, sortId, page, limit, userId);
        return data;
    }

    public async Task<AnnouncementShortDto?> GetAnnouncementShortByOfferId(Guid offerId, Guid userId)
    {
        var data = await announcementRepository.GetAnnouncementShortByOfferId(offerId, userId);
        return data;
    }

    public async Task<AnnouncementFullDto?> GetAnnouncementFullById(AnnouncementInfoCommandDto commandDto)
    {
        var result = await announcementRepository.GetAnnouncementFullById(commandDto.AnnouncementId, commandDto.UserId);
        return result;
    }
    
    public async Task<bool> SetClosedAt(Guid id)
    {
        return await announcementRepository.SetClosedAt(id);
    }

    public async Task<AnnouncementsShortAndPagesDto> GetBoughtAnnouncementsByUserId(Guid userId, int page, int limit)
    {
        return await announcementRepository.GetBoughtByUserId(userId, page, limit);
    }
    
    public async Task<AnnouncementsShortAndPagesDto> GetSoldAnnouncementsByUserId(Guid userId, int page, int limit)
    {
        return await announcementRepository.GetSoldByUserId(userId, page, limit);
    }
    
    public async Task<AnnouncementsShortAndPagesDto> GetPlacedAnnouncementsByUserId(Guid userId, int page, int limit)
    {
        return await announcementRepository.GetPlacedByUserId(userId, page, limit);
    }
    
    public async Task<Guid?> GetPropertyIdByAnnouncementIdAsync(Guid announcementId)
    {
        var result = await announcementRepository.GetPropertyIdByAnnouncementIdAsync(announcementId);
        return result;
    }
    
    public async Task<Guid?> GetStatementIdByAnnouncementIdAsync(Guid announcementId)
    {
        var result = await announcementRepository.GetStatementIdByAnnouncementIdAsync(announcementId);
        return result;
    }
    
    public async Task<List<AnnouncementGridDto>> GetAnnouncementsGrid()
    {
        var result = await announcementRepository.GetAnnouncementsGridAsync();
        return result;
    }
}