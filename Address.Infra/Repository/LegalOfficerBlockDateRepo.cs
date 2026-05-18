using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Infra;
using LCMS.Dto;
using LCMS.Persistence;
using Microsoft.EntityFrameworkCore;

using System.Net;

namespace CRM.Infra
{
    public class LegalOfficerBlockDateRepo : Repository<LegalOfficerBlockedDates>, ILegalOfficerBlockDateRepo
    {
        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;
        public LegalOfficerBlockDateRepo(LCMSDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<LegalOfficerBlockedDatesDto>> LoadLegalOfficerBlockDate(long LegalOfficerId)
        {
            List<LegalOfficerBlockedDatesDto> list = new();

            if (LegalOfficerId != 0)
            {
                var blockDates = await _dbContext.LegalOfficerBlockedDates
                    .Where(x => x.LegalOfficerId == LegalOfficerId)
                    .AsNoTracking()
                    .ToListAsync();

                list = _mapper.Map<List<LegalOfficerBlockedDatesDto>>(blockDates);
            }

            return list;
        }

        public async Task<LegalOfficerBlockedDates> InsertLegalOfficerBlockedDates(LegalOfficerBlockedDatesDto request)
        {
            var LegalOfficerBlockDates = _mapper.Map<LegalOfficerBlockedDates>(request);
            await AddAsync(LegalOfficerBlockDates);

            return LegalOfficerBlockDates;
        }

        public async Task<LegalOfficerBlockedDates> UpdateLegalOfficerBlockedDates(LegalOfficerBlockedDatesDto request)
        {
            var LegalOfficerBlockDates = await _dbContext.LegalOfficerBlockedDates.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (LegalOfficerBlockDates == null)
                throw new BusinessException("Id not found", HttpStatusCode.BadRequest);
            _mapper.Map(request, LegalOfficerBlockDates);

            return LegalOfficerBlockDates;
        }

        public async Task<LegalOfficerBlockedDatesDto> GetLegalOfficerBlockedDateByBlockDateId(long legalOfficerBlockDateId, bool isTracking = false)
        {
            var legalOfficerBlockedDate = await GetByIdAsync(legalOfficerBlockDateId, isTracking) ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);

            return _mapper.Map<LegalOfficerBlockedDatesDto>(legalOfficerBlockedDate);
        }

        public async Task<List<LegalOfficerBlockedCalenderDto>> GetLegalOfficerBlockedDateCalender(long legalOfficerId, bool isTracking = false)
        {
            if (legalOfficerId == 0)
                return new List<LegalOfficerBlockedCalenderDto>();

            return await _dbContext.LegalOfficerBlockedDates
                        .Where(x => x.LegalOfficerId == legalOfficerId)
                        .AsNoTracking()
                        .GroupBy(x => x.BlockDate)
                        .Select(g => new LegalOfficerBlockedCalenderDto
                        {
                            Date = g.Key,
                            Status = g.Any(x => x.BlockTypeConfigId == 41) ? "F" : "P"
                        })
                        .ToListAsync();
                            }

    }
}

