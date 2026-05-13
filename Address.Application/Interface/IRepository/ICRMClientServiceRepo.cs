using CRM.Domain;
using Galaxy.Application;
using Galaxy.Dto;
using LCMS.Dto;

namespace CRM.Application
{
    public interface ICRMClientServiceRepo : IRepository<CRMClientService>
    {
        Task<CRMClientService> InsertClientService(CRMClientService request);
        Task<CRMClientService> UpdateClientService(CRMClientServiceDto request);
        Task<CRMClientService> GetClientServiceById(long clientServiceId, bool isTracking = false);

        Task<CRMClientService> GenerateClientServiceRefNo(CRMClientService client);
        Task<DDLData> ClientServiceInitialData();
        Task<CRMClientServiceSectionDto> GetAllClientServiceByClientIdAsync(long clientId);
    }
}