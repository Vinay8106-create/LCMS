using CRM.Domain;
using Galaxy.Application;
using LCMS.Dto;

namespace CRM.Application
{
    public interface ICRMClientDocumentRepo : IRepository<CRMClientDocument>
    {
        Task<CRMClientDocument> InsertCRMClientDocumentAsync(CRMClientDocument request);
        Task<CRMClientDocument> UpdateCRMClientDocumentAsync(CRMClientDocument request);
        Task<CRMClientDocument> GetCRMClientDocumentDetailById(long CRMClientContactDetailId, bool isTracking = false);

        Task<CRMClientDocumentSectionDto> GetAllDocumentsByClientIdAsync(long clientId);
    }
}
