using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Application.Utils;

namespace RealEstateAgency.Application.Services;

public class PaymentService(IPaymentRepository repository, IAnnouncementsService announcementsService,
    IAuditService auditService, ApplicationMapper mapper, IUnitOfWork unitOfWork) : IPaymentService
{
    public async Task<Guid?> InsertPayment(PaymentDto paymentDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var mapped = mapper.PaymentDtoToPaymentEntity(paymentDto);

            if (!await announcementsService.SetClosedAt(paymentDto.AnnouncementId))
            {
                return Guid.Empty;
            }

            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.BuyProperty),
                UserId = paymentDto.CustomerId,
                Details = $"User: {paymentDto.CustomerId} bought from an announcement: {paymentDto.AnnouncementId}"
            };
            
            await auditService.InsertAudit(auditDto);
            await unitOfWork.CommitAsync();
            
            return await repository.Insert(mapped);
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            return null;
        }
    }
}