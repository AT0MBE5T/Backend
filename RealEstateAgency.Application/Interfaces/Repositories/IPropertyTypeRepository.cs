using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IPropertyTypeRepository
{
    Task<List<PropertyType>> GetAllAsync();
    Task<int> GetTotalPlacedAnnouncementsDate(Guid propertyTypeId, DateTime date);
    Task<int> GetTotalDealsDate(Guid propertyTypeId, DateTime date);
    Task<decimal> GetTotalIncomeDate(Guid propertyTypeId, DateTime date);
    Task<int> GetTotalPlacedAnnouncementsDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo);
    Task<int> GetTotalDealsDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo);
    Task<decimal> GetTotalIncomeDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo);
    Task<PropertyTypeTopDealCoreDto?> GetTopDealDate(Guid propertyTypeId, DateTime date);
    Task<PropertyTypeTopDealCoreDto?> GetTopDealDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo);
    Task<PropertyTypeTopRealtorCoreDto?> GetTopRealtorDate(Guid propertyTypeId, DateTime date);
    Task<PropertyTypeTopRealtorCoreDto?> GetTopRealtorDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo);
    Task<PropertyTypeTopClientCoreDto?> GetTopClientDate(Guid propertyTypeId, DateTime date);
    Task<PropertyTypeTopClientCoreDto?> GetTopClientDateSpan(Guid propertyTypeId, DateTime dateFrom, DateTime dateTo);
}