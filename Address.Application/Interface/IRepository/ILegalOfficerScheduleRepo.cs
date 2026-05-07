using Galaxy.Application;
using LCMS.Dto;
using CRM.Domain;
using Galaxy.Dto;

namespace CRM.Application
{
    public interface ILegalOfficerScheduleRepo : IRepository<LegalOfficerSchedules>
    {
        Task<LegalOfficerSchedules> InsertLegalOfficerSchedule(LegalOfficerSchedulesDto request);
        Task<LegalOfficerSchedules> UpdateLegalOfficerSchedule(LegalOfficerSchedulesDto request);
        Task<List<LegalOfficerSchedulesDto>> LoadLegalOfficerSchedule(long LegalOfficerId);
        Task<long> GetLegalOfficerIdbyUserLoginId(string UserLoginId);



    }
}