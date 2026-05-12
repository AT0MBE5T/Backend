using Microsoft.Extensions.Logging;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mappers;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Repositories;

namespace RealEstateAgency.Application.Services;

public class SupportService(
        ISupportRepository supportRepository,
        ApplicationMapper applicationMapper,
        ILogger<SupportService> logger,
        IChatService chatService,
        IUnitOfWork unitOfWork,
        IChatRepository chatRepository,
        IChatMemberRepository chatMemberRepository,
        IAuditService auditService
    ) : ISupportService
{
    private async Task<Guid?> GetOrCreateSupportChat(Guid supportId, Guid adminId)
    {
        var support = await GetByIdAsync(supportId);

        if (support is null)
            return null;

        var userId = support.UserId;
        
        var chatId = await chatRepository.GetChatByBothIdsAsync(userId, adminId);

        if (chatId is not null)
            return chatId;
        
        var objToAdd = new Chat { TypeId = Guid.Parse(ChatTypes.Support), AnnouncementId = null, SupportId = supportId};

        try
        {
            await unitOfWork.BeginTransactionAsync();
        
            var addChatResult = await chatRepository.AddChatAsync(objToAdd);

            if (addChatResult is null)
                throw new Exception($"Could not create chat {objToAdd.Id}");
            
            chatId = objToAdd.Id;

            var chatMemberUser = new ChatMember
            {
                UserId = userId,
                ChatId = chatId.Value,
            };
        
            var chatMemberResult1 = await chatMemberRepository.AddChatMemberAsync(chatMemberUser);
            
            if (!chatMemberResult1)
                throw new Exception("Could not create chat member");
            
            var chatMemberUser2 = new ChatMember
            {
                UserId = adminId,
                ChatId = chatId.Value,
            };
        
            var chatMemberResult2 = await chatMemberRepository.AddChatMemberAsync(chatMemberUser2);
            
            if (!chatMemberResult2)
                throw new Exception("Could not create chat member");
            
            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.CreatedSupportChat),
                UserId = userId,
                Details = $"Support chat is created between {adminId} and {userId}",
            };
            
            await auditService.InsertAudit(auditDto);

            await unitOfWork.CommitAsync();
            return chatId;
        }
        catch(Exception ex)
        {
            logger.LogError("Failed to get or create chat: {ex}", ex);
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
    
    public async Task<bool> IsUserHasUnclosedSupport(Guid userId)
    {
        var result = await supportRepository.IsUserHasUnclosedSupportAsync(userId);
        return result;
    }

    public async Task<List<SupportGridDto>> GetAllSupports()
    {
        var result = await supportRepository.GetAllSupportsAsync();
        return result;
    }
    
    public async Task<SupportDto?> GetByIdAsync(Guid complaintId)
    {
        var complaint = await supportRepository.GetByIdAsync(complaintId);
        
        var result = complaint is not null
            ? applicationMapper.MapSupportToDto(complaint)
            : null;

        return result;
    }

    public async Task<List<SupportGridDto>> GetAllOpenedSupports()
    {
        var result = await supportRepository.GetAllOpenedSupportsAsync();
        return result;
    }

    public async Task<Guid> InsertAsync(SupportDto support)
    {
        try
        {
            var isAlreadySupported = await IsUserHasUnclosedSupport(support.UserId);

            if (isAlreadySupported)
                return Guid.Empty;
        
            var model = applicationMapper.MapSupportDtoToEntity(support);
            var result = await supportRepository.InsertAsync(model);
            if (result == Guid.Empty) return result;
            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.CreateSupport),
                UserId = support.UserId,
                Details = $"Support is created by {support.UserId}",
            };
            
            await auditService.InsertAudit(auditDto);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to add a support: {ex}", ex);
            return Guid.Empty;
        }
    }

    public async Task<Guid?> AdminJoinAsync(Guid supportId, Guid adminId)
    {
        try
        {
            var result = await supportRepository.AdminJoinSupportAsync(supportId, adminId);
            var chatId = await GetOrCreateSupportChat(supportId, adminId);
            var support = await supportRepository.GetByIdAsync(supportId);

            if (chatId is null || chatId == Guid.Empty || support is null || !result)
                return null;
            
            await chatService.AddMessage(support.UserId, chatId.Value, support.UserNote);
            return chatId;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to update a support: {ex}", ex);
            return null;
        }
    }
    
    public async Task<bool> CloseAsync(Guid supportId, Guid userId)
    {
        try
        {
            var result = await supportRepository.CloseSupportAsync(supportId);
            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.CloseSupport),
                UserId = userId,
                Details = $"Support {supportId} is closed",
            };
            
            await auditService.InsertAudit(auditDto);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to update a support: {ex}", ex);
            return false;
        }
    }
}