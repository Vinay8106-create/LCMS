using CRM.Domain;
using Galaxy.Application;
using Galaxy.Dto;
using LCMS.Domain;
using LCMS.Dto;

namespace CRM.Application
{
    public interface ICRMClientServiceNotesRepo : IRepository<CRMClientServiceNotes>
    {
        Task<List<CRMClientServiceNotesDto>> GetClientServiceNotesById(long clientServiceId, bool isTracking = false);
    }
}