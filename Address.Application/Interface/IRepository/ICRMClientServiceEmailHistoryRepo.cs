using CRM.Domain;
using Galaxy.Application;
using Galaxy.Dto;
using LCMS.Domain;
using LCMS.Dto;

namespace CRM.Application
{
    public interface ICRMClientServiceEmailHistoryRepo : IRepository<CRMClientServiceEmailHistory>
    {
        Task<List<CRMClientServiceEmailHistoryDto>> GetClientServiceEmailHistoryById(long clientServiceId, bool isTracking = false);
    }
}