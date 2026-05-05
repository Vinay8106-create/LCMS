using CRM.Domain;
using Galaxy.Application.Repo;
using LCMS.Dto;

namespace CRM.Application
{
    public interface ICRMClientSearchRepo : ISearchRepo<CRMClient, CRMClientSearchDto, CRMClientSearchResultsDto, CRMClientSearchCombined>
    {
    }
}
