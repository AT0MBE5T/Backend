using System.Net;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Services;

public class AnnouncementsService(IAnnouncementRepository announcementRepository, IStatementsService statementService,
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

    private async Task Verify(SwitchVerificationCommand command)
    {
        var data = new Verification
        {
            Id = Guid.NewGuid(),
            CreatedBy = command.UserId,
            CreatedAt = DateTime.UtcNow,
            AnnouncementId = command.AnnouncementId,
        };

        await verificationRepository.Insert(data);
        var auditDto = new AuditDto
        {
            ActionId = Guid.Parse(AuditAction.Verificate),
            UserId = command.UserId,
            Details = $"Announcement {command.AnnouncementId} verificated by {command.UserId}"
        };
                
        await auditService.InsertAudit(auditDto);
    }
    
    private async Task Unverify(SwitchVerificationCommand command, Verification verification)
    {
        await verificationRepository.Delete(verification);
        var auditDto = new AuditDto
        {
            ActionId = Guid.Parse(AuditAction.Unverificate),
            UserId = command.UserId,
            Details = $"Announcement {command.AnnouncementId} unverificated by {command.UserId}"
        };
                
        await auditService.InsertAudit(auditDto);
    }

    public async Task<string> SwitchVerifyAnnouncement(SwitchVerificationCommand command)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var isPaid = await paymentService.IsExistByAnnouncementIdAsync(command.AnnouncementId);

            if (isPaid)
                return "Already paid";
            
            var verification = await announcementRepository.GetVerificationAsync(command.AnnouncementId);
            if (verification == null)
            {
                await Verify(command);
            }
            else
            {
                await Unverify(command, verification);
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
    
    public async Task<CloseAnnouncementResponse> CloseAnnouncement(CloseAnnouncementCommand command)
    {
        var isExist = await paymentService.IsExistByAnnouncementIdAsync(command.AnnouncementId);

        if (isExist)
            return new CloseAnnouncementResponse(Guid.Empty, "Payment is exist");
        
        var paymentDto = mapper.CloseAnnouncementToPaymentDto(command);
        
        if (!await SetClosedAt(paymentDto.AnnouncementId))
        {
            return new CloseAnnouncementResponse(Guid.Empty, "SetClosed error");
        }
        
        var paymentId = await paymentService.InsertPayment(paymentDto);
        return new CloseAnnouncementResponse(paymentId ?? Guid.Empty, string.Empty);
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
    
    public async Task<AddAnnouncementResponse> CreateAnnouncementAsync(CreateAnnouncementCommand command)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var announcementRequest = new AnnouncementRequestApp
            {
                Area = command.Area,
                Content = command.Content,
                Description = command.Description,
                Floors = command.Floors,
                Location = command.Location,
                Photos = command.Photos,
                Price = command.Price,
                PropertyType = command.PropertyType,
                Rooms = command.Rooms,
                StatementType = command.StatementType,
                Title = command.Title
            };

            var propertyDto = mapper.AnnouncementRequestToPropertyDto(announcementRequest);
            var propertyId = await propertyService.AddProperty(propertyDto);
            
            for (var i = 0; i < command.Photos.Count; i++)
            {
                var uploadResult = await imageService.UploadImageAsync(
                    command.Photos[i].Content,
                    command.Photos[i].FileName);

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
                return new AddAnnouncementResponse((int)HttpStatusCode.BadRequest, "Statement not created", Guid.Empty);
            
            var announcement = mapper.AnnouncementRequestToAnnouncementDto(announcementRequest, (Guid)statementId, true);
            var announcementId = await AddAnnouncement(command.UserId, announcement);

            if (announcementId is null)
                return new AddAnnouncementResponse((int)HttpStatusCode.BadRequest, "Announcement not created", Guid.Empty);
            
            var offerDto = await announcementRepository.GetAnnouncementShortByOfferId((Guid)announcementId, command.UserId);
            await hubService.NotifyNewOfferAsync(offerDto);

            return new AddAnnouncementResponse((int)HttpStatusCode.Created, string.Empty, (Guid)announcementId);
        }
        catch(Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new AddAnnouncementResponse((int)HttpStatusCode.BadRequest, ex.Message, Guid.Empty);
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
    
    public async Task<string> UpdateAnnouncementAsync(AnnouncementUpdateCommand command)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var request = new AnnouncementEditRequestApp
            {
                Area = command.Area,
                Content = command.Content,
                Description = command.Description,
                Floors = command.Floors,
                Location = command.Location,
                UserId = command.UserId,
                AnnouncementId = command.AnnouncementId,
                DeletedImageIds = command.DeletedImageIds,
                ExistingImageOrder = command.ExistingImageOrder,
                NewPhotos = command.NewPhotos,
                Price = command.Price,
                PropertyType = command.PropertyType,
                Rooms = command.Rooms,
                StatementType = command.StatementType,
                Title = command.Title
            };
            
            var statementId = await GetStatementIdByAnnouncementIdAsync(command.AnnouncementId);
            var propertyId = await GetPropertyIdByAnnouncementIdAsync(command.AnnouncementId);

            if (statementId == null || propertyId == null)
            {
                return "Statement or property not found";
            }
            
            var photos = command.NewPhotos?.ToList() ??  [];
            var deleteImages = command.DeletedImageIds ?? [];
            var existingImageOrder = command.ExistingImageOrder ?? [];

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
            var statementRes = await statementService.UpdateStatementAsync((Guid)statementId, command.UserId, statement);
            
            if (!statementRes)
            {
                return "Statement not updated";
            }
            
            var announcement = mapper.AnnouncementEditRequestToAnnouncementDto(request, (Guid)statementId, true);
            var response = await UpdateAsync(command.AnnouncementId, announcement);
            
            var offerDto = await GetAnnouncementShortByOfferId(command.AnnouncementId, command.UserId);

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
        
    public async Task<string> DeleteAnnouncementAsync(DeleteAnnouncementCommand command)
    {
        var isPaid = await paymentService.IsExistByAnnouncementIdAsync(command.AnnouncementId);

        if (isPaid)
            return "Announcement already paid";

        var result = await DeleteAsync(command.AnnouncementId, command.UserId);

        await hubService.DeleteOfferAsync(command.AnnouncementId);

        return string.Empty;
    }

    public async Task<Guid> GetAuthorOfferIdByQuestionId(Guid questionId)
    {
        var data = await announcementRepository.GetAuthorOfferIdByQuestionIdAsync(questionId);
        return data;
    }
    
    public async Task<AnnouncementsShortAndPages> GetSearchDataPaginated(string text, List<string> filters, int sortId, int page, int limit, Guid userId)
    {
        var data = await announcementRepository.GetSearchData(text, filters, sortId, page, limit, userId);
        return data;
    }

    public async Task<AnnouncementShort?> GetAnnouncementShortByOfferId(Guid offerId, Guid userId)
    {
        var data = await announcementRepository.GetAnnouncementShortByOfferId(offerId, userId);
        return data;
    }

    public async Task<AnnouncementFull?> GetAnnouncementFullById(AnnouncementInfoCommand command)
    {
        var result = await announcementRepository.GetAnnouncementFullById(command.AnnouncementId, command.UserId);
        return result;
    }
    
    public async Task<bool> SetClosedAt(Guid id)
    {
        return await announcementRepository.SetClosedAt(id);
    }

    public async Task<AnnouncementsShortAndPages> GetBoughtAnnouncementsByUserId(Guid userId, int page, int limit)
    {
        return await announcementRepository.GetBoughtByUserId(userId, page, limit);
    }
    
    public async Task<AnnouncementsShortAndPages> GetSoldAnnouncementsByUserId(Guid userId, int page, int limit)
    {
        return await announcementRepository.GetSoldByUserId(userId, page, limit);
    }
    
    public async Task<AnnouncementsShortAndPages> GetPlacedAnnouncementsByUserId(Guid userId, int page, int limit)
    {
        return await announcementRepository.GetPlacedByUserId(userId, page, limit);
    }
    
    public async Task<byte[]> GetBytesByAnnouncementIdAsync(Guid announcementId)
    {
        var result = await announcementRepository.GetBytesByAnnouncementIdAsync(announcementId);
        return result;
    }
    
    public async Task<Guid> GetImageIdByAnnouncementIdAsync(Guid announcementId)
    {
        var result = await announcementRepository.GetImageIdByAnnouncementIdAsync(announcementId);
        return result;
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
    
    public async Task<List<AnnouncementGrid>> GetAnnouncementsGrid()
    {
        var result = await announcementRepository.GetAnnouncementsGridAsync();
        return result;
    }
}