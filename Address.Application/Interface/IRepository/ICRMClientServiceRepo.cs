using Galaxy.Application;
using LCMS.Dto;
using CRM.Domain;
using Galaxy.Dto;

namespace CRM.Application
{
    public interface ICRMClientServiceRepo : IRepository<CRMClientService>
    {
        Task<CRMClientService> InsertClientService(CRMClientService request);
        Task<CRMClientService> UpdateClientService(CRMClientServiceDto request);
        Task<CRMClientService> GetClientServiceById(long clientServiceId, bool isTracking = false);

        Task<CRMClientService> GenerateClientServiceRefNo(CRMClientService client);
        Task<DDLData> ClientServiceInitialData();

    }
}