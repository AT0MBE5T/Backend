using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using ApplicationMapper = RealEstateAgency.Application.Mappers.ApplicationMapper;

namespace RealEstateAgency.Application.Services;

public class PaymentService(IPaymentRepository repository,
    IAuditService auditService, ApplicationMapper mapper, IUnitOfWork unitOfWork) : IPaymentService
{
    public async Task<Guid?> InsertPayment(PaymentDto paymentDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var mapped = mapper.PaymentDtoToPaymentEntity(paymentDto);

            var auditDto = new AuditDto
            {
                ActionId = Guid.Parse(AuditAction.BuyProperty),
                UserId = paymentDto.CustomerId,
                Details = $"User: {paymentDto.CustomerId} bought from an announcement: {paymentDto.AnnouncementId}"
            };
            
            var paymentId =  await repository.Insert(mapped);
            await auditService.InsertAudit(auditDto);
            await unitOfWork.CommitAsync();
            return paymentId;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            return null;
        }
    }

    public async Task<bool> IsExistByAnnouncementIdAsync(Guid announcementId)
    {
        var result = await repository.IsExistByAnnouncementId(announcementId);
        return result;
    }
}