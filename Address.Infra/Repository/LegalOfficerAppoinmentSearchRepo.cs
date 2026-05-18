using CRM.Application;
using CRM.Domain;
using Galaxy.Infra.Repository;
using LCMS.DTO;
using LCMS.Persistence;

namespace CRM.Infra
{
    public class LegalOfficerAppoinmentSearchRepo : BaseSearchRepo<LegalOfficerAppoinment, LegalOfficerAppoinmentSearchDto, LegalOfficerAppoinmentSearchResultsDto, LegalOfficerAppoinmentSearchCombined>, ILegalOfficerAppoinmentSearchRepo
    {
        protected readonly LCMSDbContext _context;

        public LegalOfficerAppoinmentSearchRepo(LCMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public override IQueryable<LegalOfficerAppoinmentSearchCombined> BuildInitialQuery(IQueryable<LegalOfficerAppoinment> queryable)
        {
            var baseQuery = from legalOfficerAppoinment in _context.LegalOfficerAppoinmentSearch
                            select new LegalOfficerAppoinmentSearchCombined
                            {
                                LegalOfficerAppoinmentViewModel = legalOfficerAppoinment
                            };

            return baseQuery;
        }

        protected override IQueryable<LegalOfficerAppoinmentSearchResultsDto> ProjectResults(IQueryable<LegalOfficerAppoinmentSearchCombined> query)
        {
            return query.Select(x => new LegalOfficerAppoinmentSearchResultsDto
            {
                LegalOfficerAppoinmentId = x.LegalOfficerAppoinmentViewModel.LegalOfficerAppoinmentId,
                AppoinmentNo = x.LegalOfficerAppoinmentViewModel.AppoinmentNo,
                ClientName = x.LegalOfficerAppoinmentViewModel.ClientName,
                LegalOfficerName = x.LegalOfficerAppoinmentViewModel.LegalOfficerName,
                FromDate = x.LegalOfficerAppoinmentViewModel.FromDate,
                AppoinmentStatusConfigId = x.LegalOfficerAppoinmentViewModel.AppoinmentStatusConfigId,
                AppoinmentStatus = x.LegalOfficerAppoinmentViewModel.AppoinmentStatus,
                Notes = x.LegalOfficerAppoinmentViewModel.Notes,
                StartTime = x.LegalOfficerAppoinmentViewModel.StartTime,
                EndTime = x.LegalOfficerAppoinmentViewModel.EndTime
            });
        }
    }
}
