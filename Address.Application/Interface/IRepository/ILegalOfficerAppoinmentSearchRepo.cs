using CRM.Domain;
using Galaxy.Application.Repo;
using LCMS.DTO;

namespace CRM.Application
{
    public interface ILegalOfficerAppoinmentSearchRepo : ISearchRepo<LegalOfficerAppoinment, LegalOfficerAppoinmentSearchDto, LegalOfficerAppoinmentSearchResultsDto, LegalOfficerAppoinmentSearchCombined>
    {
    }
}
