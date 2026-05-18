using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Dto;
using Galaxy.Infra;
using LCMS.Constants;
using LCMS.Dto;
using LCMS.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CRM.Infra
{
    public class LegalOfficerAppoinmentRepo : Repository<LegalOfficerAppoinment>, ILegalOfficerAppoinmentRepo
    {
        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;

        public LegalOfficerAppoinmentRepo(LCMSDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private static SemaphoreSlim _appointmentRefNoLock = new SemaphoreSlim(1, 1);

        public async Task<LegalOfficerAppoinment> GenerateAppointmentRefNo(LegalOfficerAppoinment appointment)
        {
            await _appointmentRefNoLock.WaitAsync();
            try
            {
                string appointmentRefNo = null;

                using var reader = await _dbContext.ExecuteSpAsync(CRMConstants.StoredProcedures.APP_SP_GetAppoinmentRefNumber);

                var resultSets = await reader.ReadAsync();
                appointmentRefNo = resultSets
                    .Select(x => x.AppointmentReferenceNumber)
                    .FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(appointmentRefNo))
                    appointment.AppointmentNo = appointmentRefNo;

                return appointment;
            }
            finally
            {
                _appointmentRefNoLock.Release();
            }
        }

        public async Task<DDLData> GetLegalOfficerAppoinmentInitialData()
        {
            DDLData dDLData = new DDLData();
            var appoinmentStatus = await GetDDLAsync<config_AppoinmentStatus>("DDLAppoinmentStatus");
            dDLData.data.Add(appoinmentStatus);
            var priorityLevel = await GetDDLAsync<config_PriorityLevel>("DDLPriorityLevel");
            dDLData.data.Add(priorityLevel);
            var meetingType = await GetDDLAsync<config_MeetingType>("DDLMeetingType");
            dDLData.data.Add(meetingType);

            return dDLData;
        }

        public async Task<DDL> GetDDLAsync<TEntity>(string key) where TEntity : class
        {
            var data = await _dbContext.Set<TEntity>()
                .Select(x => new DDLClass
                {
                    Id = EF.Property<int>(x, "ConfigId"),
                    Description = EF.Property<string>(x, "Description")
                }).AsNoTracking().ToListAsync();

            return new DDL

            {
                Key = key,
                Value = data
            };
        }

        public async Task<bool> IsSlotAlreadyBookedAsync(long officerId, DateTime date, TimeSpan? start, TimeSpan? end)
        {
            return await _dbContext.LegalOfficerAppoinment
                .AnyAsync(a =>
                    a.LegalOfficerId == officerId &&
                    a.AppoinmentDate == date &&
                    a.IsBooked == "Y" &&
                    a.StartTime < end &&
                    a.EndTime > start
                );
        }

        public async Task<LegalOfficerAppoinment> InsertLegalOfficerAppoinmentAsync(LegalOfficerAppoinment request)
        {
            await AddAsync(request);

            return request;
        }

        public async Task<LegalOfficerAppoinment> UpdateLegalOfficerAppoinmentAsync(LegalOfficerAppoinmentDto request)
        {
            var appointment = await _dbContext.LegalOfficerAppoinment
                .FirstOrDefaultAsync(x => x.Id == request.Id)
                ?? throw new BusinessException("Appointment not found", HttpStatusCode.BadRequest);

            _mapper.Map(request, appointment);

            return appointment;
        }

        public async Task<List<AppoinmentCalendarDto>> GetAppoinmentCalendarAsync(long legalOfficerId, int month, int year)
        {
            var parameters = new {
                LegalOfficerId = legalOfficerId,
                Month = month,
                Year = year
            };

            using var reader = await _dbContext.ExecuteSpAsync(
                CRMConstants.StoredProcedures.APP_SP_GetAppoinmentCalendarByMonth,
                parameters);

            var resultSets = await reader.ReadAsync();

            return resultSets.Select(x => new AppoinmentCalendarDto
            {
                AppoinmentDate = x.AppoinmentDate != null
                 ? ((DateTime)x.AppoinmentDate).ToString("yyyy-MM-dd") : null,
                TotalAppointments = (int)x.TotalAppointments,
                BookedCount = (int)x.BookedCount,
                AvailableCount = (int)x.AvailableCount,
                PendingCount = (int)x.PendingCount,
                DayStatus = (string)x.DayStatus
            }).ToList();
        }


        public async Task<List<LegalOfficerAppoinment>> GetAppoinmentTimeSlotsByDateAsync(long legalOfficerId, DateTime date)
        {
            return await _dbContext.LegalOfficerAppoinment
                .Where(a => a.LegalOfficerId == legalOfficerId &&
                            a.AppoinmentDate == date)
                .OrderBy(a => a.StartTime)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
