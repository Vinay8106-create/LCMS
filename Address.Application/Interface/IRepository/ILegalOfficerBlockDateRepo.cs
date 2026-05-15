using CRM.Domain;
using Galaxy.Application;
using LCMS.Dto;

namespace CRM.Application
{
    public interface ILegalOfficerBlockDateRepo : IRepository<LegalOfficerBlockedDates>
    {
        Task<LegalOfficerBlockedDates> InsertLegalOfficerBlockedDates(LegalOfficerBlockedDatesDto request);
        Task<LegalOfficerBlockedDates> UpdateLegalOfficerBlockedDates(LegalOfficerBlockedDatesDto request);
        Task<List<LegalOfficerBlockedDatesDto>> LoadLegalOfficerBlockDate(long LegalOfficerId);
        Task<LegalOfficerBlockedDatesDto> GetLegalOfficerBlockedDateByBlockDateId(long legalOfficerBlockDateId, bool isTracking = false);
    }
}