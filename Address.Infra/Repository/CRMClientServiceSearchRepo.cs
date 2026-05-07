using CRM.Application;
using CRM.Domain;
using Galaxy.Infra.Repository;
using LCMS.DTO;
using LCMS.Persistence;

namespace CRM.Infra
{
    public class CRMClientServiceSearchRepo : BaseSearchRepo<CRMClientService, CRMClientServiceSearchDto, CRMClientServiceSearchResultsDto, CRMClientServiceSearchCombined>, ICRMClientServiceSearchRepo
    {
        protected readonly LCMSDbContext _context;

        public CRMClientServiceSearchRepo(LCMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public override IQueryable<CRMClientServiceSearchCombined> BuildInitialQuery(IQueryable<CRMClientService> queryable)
        {
            var baseQuery = from CRMClientServiceView in _context.CRMClientServiceSearch
                            select new CRMClientServiceSearchCombined
                            {
                                ClientServiceViewModel = CRMClientServiceView
                            };

            return baseQuery;
        }

        protected override IQueryable<CRMClientServiceSearchResultsDto> ProjectResults(IQueryable<CRMClientServiceSearchCombined> query)
        {
            return query.Select(x => new CRMClientServiceSearchResultsDto
            {
                ClientServiceId = x.ClientServiceViewModel.ClientServiceId,
                ClientServiceRefNo = x.ClientServiceViewModel.ClientServiceRefNo,
                ClientRefNo = x.ClientServiceViewModel.ClientRefNo ?? "-",
                ClientName = x.ClientServiceViewModel.ClientName ?? "-",
                ServiceType = x.ClientServiceViewModel.ServiceType ?? "-",
                ServiceSubType = x.ClientServiceViewModel.ServiceSubType ?? "-",
                ContactMode = x.ClientServiceViewModel.ContactMode ?? "-",
                ServiceStatus = x.ClientServiceViewModel.ServiceStatus ?? "-",
                EnteredOn = x.ClientServiceViewModel.EnteredOn,
            });
        }
    }
}
