using Galaxy.Application;
using LCMS.Dto;
using CRM.Domain;
using Galaxy.Dto;

namespace CRM.Application
{
    public interface ILegalOfficerBlockDateRepo : IRepository<LegalOfficerBlockedDates>
    {
        Task<LegalOfficerBlockedDates> InsertLegalOfficerBlockedDates(LegalOfficerBlockedDatesDto request);
        Task<LegalOfficerBlockedDates> UpdateLegalOfficerBlockedDates(LegalOfficerBlockedDatesDto request);
        Task<List<LegalOfficerBlockedDatesDto>> LoadLegalOfficerBlockDate(long LegalOfficerId);
        



    }
}