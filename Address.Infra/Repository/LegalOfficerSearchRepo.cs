using CRM.Application;
using CRM.Domain;
using Galaxy.Infra.Repository;
using LCMS.DTO;
using LCMS.Persistence;

namespace CRM.Infra
{
    public class LegalOfficerSearchRepo : BaseSearchRepo<LegalOfficer, LegalOfficerSearchDto, LegalOfficerSearchResultsDto, LegalOfficerSearchCombined>, ILegalOfficerSearchRepo
    {
        protected readonly LCMSDbContext _context;

        public LegalOfficerSearchRepo(LCMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public override IQueryable<LegalOfficerSearchCombined> BuildInitialQuery(IQueryable<LegalOfficer> queryable)
        {
            var baseQuery = from legalOfficer in _context.LegalOfficerSearch
                            select new LegalOfficerSearchCombined
                            {
                                LegalOfficerViewModel = legalOfficer
                            };

            return baseQuery;
        }

        protected override IQueryable<LegalOfficerSearchResultsDto> ProjectResults(IQueryable<LegalOfficerSearchCombined> query)
        {
            return query.Select(x => new LegalOfficerSearchResultsDto
            {
                LegalOfficerId = x.LegalOfficerViewModel.LegalOfficerId,
                Officer = x.LegalOfficerViewModel.Officer,
                RegNo = x.LegalOfficerViewModel.RegNo,
                Designation = x.LegalOfficerViewModel.Designation,
                Specialization = x.LegalOfficerViewModel.Specialization,
                ExpYears = x.LegalOfficerViewModel.ExpYears,
            });
        }
    }
}
