using CRM.Domain;
using Galaxy.Application.Repo;
using LCMS.DTO;

namespace CRM.Application
{
    public interface ICRMClientServiceSearchRepo : ISearchRepo<CRMClientService, CRMClientServiceSearchDto, CRMClientServiceSearchResultsDto, CRMClientServiceSearchCombined>
    {
    }
}
