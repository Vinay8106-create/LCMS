using CRM.Domain;
using Galaxy.Application.Repo;
using LCMS.DTO;

namespace CRM.Application
{
    public interface ILegalOfficerSearchRepo : ISearchRepo<LegalOfficer, LegalOfficerSearchDto, LegalOfficerSearchResultsDto, LegalOfficerSearchCombined>
    {
    }
}
