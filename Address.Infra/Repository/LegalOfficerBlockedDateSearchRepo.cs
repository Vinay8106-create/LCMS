using CRM.Application;
using CRM.Domain;
using Galaxy.Infra.Repository;
using LCMS.DTO;
using LCMS.Persistence;

namespace CRM.Infra
{
    public class LegalOfficerBlockedDateSearchRepo : BaseSearchRepo<LegalOfficerBlockedDates, LegalOfficerBlockedDateSearchDto, LegalOfficerBlockedDateSearchResultsDto, LegalOfficerBlockedDateSearchCombined>, ILegalOfficerBlockedDateSearchRepo
    {
        protected readonly LCMSDbContext _context;

        public LegalOfficerBlockedDateSearchRepo(LCMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public override IQueryable<LegalOfficerBlockedDateSearchCombined> BuildInitialQuery(IQueryable<LegalOfficerBlockedDates> queryable)
        {
            var baseQuery = from legalOfficerBlockedDates in _context.BlockedDateSearch
                            select new LegalOfficerBlockedDateSearchCombined
                            {
                                BlockedDateViewModel = legalOfficerBlockedDates
                            };

            return baseQuery;
        }

        protected override IQueryable<LegalOfficerBlockedDateSearchResultsDto> ProjectResults(IQueryable<LegalOfficerBlockedDateSearchCombined> query)
        {
            return query.Select(x => new LegalOfficerBlockedDateSearchResultsDto
            {
                LegalOfficerBlockedDateId = x.BlockedDateViewModel.LegalOfficerBlockedDateId,
                FromDate = Convert.ToString(x.BlockedDateViewModel.FromDate),
                ToDate = Convert.ToString(x.BlockedDateViewModel.ToDate),
                Type = x.BlockedDateViewModel.Type
            });
        }
    }
}
