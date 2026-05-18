using CRM.Domain;
using Galaxy.Application;
using Galaxy.Dto;
using LCMS.Dto;

namespace CRM.Application
{
    public interface ILegalOfficerAppoinmentRepo : IRepository<LegalOfficerAppoinment>
    {
        Task<DDLData> GetLegalOfficerAppoinmentInitialData();
        Task<LegalOfficerAppoinment> InsertLegalOfficerAppoinmentAsync(LegalOfficerAppoinment request);
        Task<bool> IsSlotAlreadyBookedAsync(long officerId, DateTime date, TimeSpan? start, TimeSpan? end);
        Task<LegalOfficerAppoinment> UpdateLegalOfficerAppoinmentAsync(LegalOfficerAppoinmentDto request);
        Task<LegalOfficerAppoinment> GenerateAppointmentRefNo(LegalOfficerAppoinment appointment);
        Task<List<AppoinmentCalendarDto>> GetAppoinmentCalendarAsync(long legalOfficerId, int month, int year);
        Task<List<LegalOfficerAppoinment>> GetAppoinmentTimeSlotsByDateAsync(long legalOfficerId, DateTime date);
    }
}
