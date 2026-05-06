using Galaxy.Application;
using LCMS.Dto;
using CRM.Domain;
using Galaxy.Dto;

namespace CRM.Application
{
    public interface ILegalOfficerRepo : IRepository<LegalOfficer>
    {
        Task<LegalOfficer> InsertLegalOfficer(LegalOfficerDto request);
        Task<LegalOfficer> UpdateLegalOfficer(LegalOfficerDto request);
        Task<LegalOfficer> GetLegalOfficerById(long LegalOfficerId, bool isTracking = false);
        Task<DDLData> GetLegalOfficerInitialData();
     



    }
}