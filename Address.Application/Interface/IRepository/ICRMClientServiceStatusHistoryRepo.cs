using CRM.Domain;
using Galaxy.Application;
using Galaxy.Dto;
using LCMS.Domain;
using LCMS.Dto;

namespace CRM.Application
{
    public interface ICRMClientServiceStatusHistoryRepo : IRepository<CRMClientServiceStatusHistory>
    {
        Task<List<CRMClientServiceStatusHistoryDto>> GetClientServiceStatusHistoryById(long clientServiceId, bool isTracking = false);
    }
}