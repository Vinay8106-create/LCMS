using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Domain.Models;
using Galaxy.Infra;
using LCMS.Dto;
using LCMS.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CRM.Infra
{
    public class LegalOfficerScheduleRepo : Repository<LegalOfficerSchedules>, ILegalOfficerScheduleRepo
    {
        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;
        public LegalOfficerScheduleRepo(LCMSDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<List<LegalOfficerSchedulesDto>> LoadLegalOfficerSchedule(long LegalOfficerId)
        {
            List<LegalOfficerSchedulesDto> list = new List<LegalOfficerSchedulesDto>();
            if (LegalOfficerId != 0)
            {
                var schedules = await _dbContext.LegalOfficerSchedules
                                      .Where(x => x.LegalOfficerId == LegalOfficerId)
                                      .AsNoTracking()
                                      .ToListAsync();

                list = Enumerable.Range(0, 7)
                   .Select(day => {
                       var existing = schedules
                           .FirstOrDefault(x => x.DayOffWeek == day);

                       return new LegalOfficerSchedulesDto
                       {
                           Id = existing?.Id ?? 0,
                           LegalOfficerId = LegalOfficerId,
                           DayOffWeek = day,
                           Dayname =
                               day == 1 ? "Monday" :
                               day == 2 ? "Tuesday" :
                               day == 3 ? "Wednesday" :
                               day == 4 ? "Thursday" :
                               day == 5 ? "Friday" :
                               day == 6 ? "Saturday" :
                                day == 0 ? "Sunday" : "",


                           StartTime = existing?.StartTime.ToString(),
                           EndTime = existing?.EndTime.ToString(),
                           SlotDuration = existing?.SlotDuration,
                           ISActive = existing?.ISActive == "Y" ? "Y" : "N",
                           BreakStartTime = Convert.ToString(existing?.BreakStartTime),
                           BreakEndTime = Convert.ToString(existing?.BreakEndTime),
                           Version = existing?.Version ?? 0

                       };
                   })
                   .ToList();
            }
            return list;


        }

        public async Task<LegalOfficerSchedules> InsertLegalOfficerSchedule(LegalOfficerSchedulesDto request)
        {
            var LegalOfficerSchedules = _mapper.Map<LegalOfficerSchedules>(request);
            await AddAsync(LegalOfficerSchedules);

            return LegalOfficerSchedules;
        }

        public async Task<LegalOfficerSchedules> UpdateLegalOfficerSchedule(LegalOfficerSchedulesDto request)
        {
            var LegalOfficerSchedules = await _dbContext.LegalOfficerSchedules.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (LegalOfficerSchedules == null)
                throw new BusinessException("Id not found", HttpStatusCode.BadRequest);
            _mapper.Map(request, LegalOfficerSchedules);

            return LegalOfficerSchedules;
        }

        public async Task<long> GetLegalOfficerIdbyUserLoginId(string userLoginId)
        {
            var legalOfficerId = await (
                from u in _dbContext.Set<User>()
                join lo in _dbContext.LegalOfficer
                    on u.Id equals lo.UserSerialId
                where u.UserLoginId == userLoginId
                select lo.Id
            ).FirstOrDefaultAsync();

            return legalOfficerId;
        }
    }
}

