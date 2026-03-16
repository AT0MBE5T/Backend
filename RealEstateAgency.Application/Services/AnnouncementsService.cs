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
    ApplicationMapper mapper, IVerificationRepository verificationRepository, IUnitOfWork unitOfWork) : IAnnouncementsService
{
    private const int Pagesize = 8;
    
    public async Task<List<AnnouncementDto>> GetAllAnnouncementsPaginated(int page)
    {
        var res = await announcementRepository.GetAllPaginatedAsync(page, Pagesize);

        if (res.Count == 0)
        {
            return [];
        }
        
        var mapped = res
            .Select(mapper.AnnouncementEntityToAnnouncementDto)
            .ToList();
        
        return mapped;
    }

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

    public async Task<bool> SwitchVerificateAnnouncement(Guid announcementId, Guid userId)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var verification = await announcementRepository.GetVerificationAsync(announcementId);
            if (verification == null)
            {
                var data = new Verification
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow,
                    AnnouncementId = announcementId,
                };

                await verificationRepository.Insert(data);
                var auditDto = new AuditDto
                {
                    ActionId = Guid.Parse(AuditAction.Verificate),
                    UserId = userId,
                    Details = $"Announcement {announcementId} verificated by {userId}"
                };
                
                await auditService.InsertAudit(auditDto);
            }
            else
            {
                await verificationRepository.Delete(verification);
                var auditDto = new AuditDto
                {
                    ActionId = Guid.Parse(AuditAction.Unverificate),
                    UserId = userId,
                    Details = $"Announcement {announcementId} unverificated by {userId}"
                };
                
                await auditService.InsertAudit(auditDto);
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

    public async Task<Guid?> AddAnnouncement(Guid userId, AnnouncementDto announcementDto)
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
                Details = $"Announcement {entity.Id} unverified by {userId}"
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
    
    public async Task<bool> UpdateAnnouncementAsync(Guid announcementId, Guid userId, AnnouncementDto announcementDto)
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

            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.UpdateAnnouncement),
                UserId = userId,
                Details = $"Announcement {announcement.Id} updated by {userId}"
            };
            
            await auditService.InsertAudit(auditDto);
            
            await unitOfWork.CommitAsync();
            return true;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> DeleteAnnouncementAsync(Guid announcementId, Guid userId)
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
    
    public async Task<AnnouncementsShortAndPages> GetSearchDataPaginated(string text, List<string> filters, int sortId, int page, int limit, Guid? userId)
    {
        var data = await announcementRepository.GetSearchData(text, filters, sortId, page, limit, userId);
        return data;
    }

    public async Task<AnnouncementShort?> GetAnnouncementShortByOfferId(Guid offerId, Guid userId)
    {
        var data = await announcementRepository.GetAnnouncementShortByOfferId(offerId, userId);
        return data;
    }

    public async Task<AnnouncementFull?> GetAnnouncementFullById(Guid id, Guid? userId)
    {
        var result = await announcementRepository.GetAnnouncementFullById(id, userId);
        return result;
    }

    public async Task<int> GetPages()
    {
        var cnt = await announcementRepository.GetAmount();
        var pages = Math.Ceiling((double)cnt / Pagesize);
        return (int)pages;
    }
    
    public async Task<bool> SetClosedAt(Guid id)
    {
        return await announcementRepository.SetClosedAt(id);
    }

    public async Task<AnnouncementsShortAndPages> GetBoughtAnnouncementsByUserId(Guid userId, int page)
    {
        return await announcementRepository.GetBoughtByUserId(userId, page, Pagesize);
    }
    
    public async Task<AnnouncementsShortAndPages> GetSoldAnnouncementsByUserId(Guid userId, int page)
    {
        return await announcementRepository.GetSoldByUserId(userId, page, Pagesize);
    }
    
    public async Task<AnnouncementsShortAndPages> GetPlacedAnnouncementsByUserId(Guid userId, int page)
    {
        return await announcementRepository.GetPlacedByUserId(userId, page, Pagesize);
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
}