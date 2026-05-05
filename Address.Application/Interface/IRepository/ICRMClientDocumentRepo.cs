using Galaxy.Application;
using LCMS.Dto;
using CRM.Domain;

namespace CRM.Application
{
    public interface ICRMClientDocumentRepo : IRepository<CRMClientDocument>
    {
        Task<CRMClientDocument> InsertCRMClientDocumentDetailAsync(CRMClientDocument request);
        Task<CRMClientDocument> UpdateCRMClientDcumentDetailAsync(CRMClientDocument request);
        Task<CRMClientDocument> GetCRMClientDocumentDetailById(long CRMClientContactDetailId, bool isTracking = false);

    }
}
