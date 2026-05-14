using CRM.Domain;
using Galaxy.Application.Repo;
using LCMS.DTO;

namespace CRM.Application
{
    public interface ILegalOfficerBlockedDateSearchRepo : ISearchRepo<LegalOfficerBlockedDates, LegalOfficerBlockedDateSearchDto, LegalOfficerBlockedDateSearchResultsDto, LegalOfficerBlockedDateSearchCombined>
    {
    }
}
