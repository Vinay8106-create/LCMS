using CRM.Domain;
using Galaxy.Application;
using Galaxy.Dto;
using LCMS.Domain;
using LCMS.Dto;

namespace CRM.Application
{
    public interface ICRMClientServiceAssignedOfficerHistoryRepo : IRepository<CRMClientServiceAssignedOfficer>
    {
        Task<List<CRMClientServiceAssignedOfficerHistoryDto>> GetClientServiceAssignedOfficerHistoryById(long clientServiceId, bool isTracking = false);
    }
}