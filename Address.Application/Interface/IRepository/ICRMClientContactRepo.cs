using CRM.Domain;
using Galaxy.Application;
using LCMS.Dto;

namespace CRM.Application
{
    public interface ICRMClientContactRepo : IRepository<CRMClientContact>
    {
        Task<CRMClientContact> InsertCRMClientContactAsync(CRMClientContactDto request);
        Task<CRMClientContact> UpdateCRMClientContactAsync(CRMClientContactDto request);
        Task<CRMClientContact> GetCRMClientContactByClientContactId(long CRMClientContactDetailId, bool isTracking = false);
    }
}
