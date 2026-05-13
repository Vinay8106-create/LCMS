using CRM.Domain;
using Galaxy.Application;
using Galaxy.Dto;
using LCMS.Dto;

namespace CRM.Application
{
    public interface ILegalOfficerRepo : IRepository<LegalOfficer>
    {
        Task<LegalOfficer> InsertLegalOfficer(LegalOfficer request);
        Task<LegalOfficer> UpdateLegalOfficer(LegalOfficerDto request);
        Task<LegalOfficerDto> GetLegalOfficerById(long LegalOfficerId, bool isTracking = false);
        Task<DDLData> GetLegalOfficerInitialData();

        Task<DDLClass> GetDetailsFromITGUser(long UserSerialId);
    }
}