using CRM.Application;
using CRM.Domain;
using Galaxy.Infra.Repository;
using LCMS.Dto;
using LCMS.Persistence;

namespace CRM.Infra
{
    public class CRMClientSearchRepo : BaseSearchRepo<CRMClient, CRMClientSearchDto, CRMClientSearchResultsDto, CRMClientSearchCombined>, ICRMClientSearchRepo
    {
        protected readonly LCMSDbContext _context;

        public CRMClientSearchRepo(LCMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public override IQueryable<CRMClientSearchCombined> BuildInitialQuery(IQueryable<CRMClient> queryable)
        {
            var baseQuery = from CRMClientView in _context.CRMClientSearch
                            select new CRMClientSearchCombined
                            {
                                ClientViewModel = CRMClientView
                            };

            return baseQuery;
        }

        protected override IQueryable<CRMClientSearchResultsDto> ProjectResults(IQueryable<CRMClientSearchCombined> query)
        {
            return query.Select(x => new CRMClientSearchResultsDto
            {
                ClientId = x.ClientViewModel.ClientId,
                ClientRefNo = x.ClientViewModel.ClientRefNo,
                ClientName = x.ClientViewModel.ClientName ?? "-",

                ClientType = x.ClientViewModel.ClientType ?? "-",
                TypeConfigId = x.ClientViewModel.ClientTypeConfigId,
                ClientSubType = x.ClientViewModel.ClientSubType ?? "-",
                ClientSubTypeConfigId = x.ClientViewModel.ClientSubTypeConfigId,
                ContactNo = x.ClientViewModel.ContactNo ?? "-",
                EmailId = x.ClientViewModel.EmailId ?? "-",
                Status = x.ClientViewModel.ClientStatus ?? "-",
                StatusConfigId = x.ClientViewModel.ClientStatusConfigId,
            });
        }
    }
}
