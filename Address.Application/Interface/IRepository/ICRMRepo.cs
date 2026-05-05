using Galaxy.Application;
using LCMS.Dto;
using CRM.Domain;

namespace CRM.Application
{
    public interface ICRMClientRepo : IRepository<CRMClient>
    {
        Task<CRMClient> InsertClient(CRMClient request);
        Task<CRMClient> UpdateClient(CRMClientDto request);
        Task<CRMClient> GetClientById(long clientId, bool isTracking = false);
       
        Task<CRMClient> GenerateClientRefNo(CRMClient client);
        Task<CRMClient> GetAlreadyExist(string contactNo, string emailId);
        Task<CRMClient> GetClientByClientRefNo(string ClientRefNo, CancellationToken cancellationToken = default);
    }
}