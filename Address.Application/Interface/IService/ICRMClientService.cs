using Galaxy.Dto;
using LCMS.Dto;
using LCMS.DTO;

namespace CRM.Application
{
    public interface ICRMClientService
    {
        #region Client
        Task<CRMClientSearchDto> GetClientSearchAsync();
        Task<SearchResult<CRMClientSearchResultsDto>> SearchCRMClient(CRMClientSearchDto request);
        Task<DDLData> GetClientInitialDataAsync();
        Task<CRMClientDto> GetClientByClientIdAsync(long clientId);
        Task<List<LegalOfficerAppoinmentDto>> GetAppointmentsByClientIdAsync(long clientId);
        Task<CRMClientDto> CreateClientAsync();
        Task<CRMClientDto> SaveClientAsync(CRMClientDto request);



        #endregion

        #region Client Contact
        Task<CRMClientContactDto> CreateClientContactAsync();
        Task<CRMClientContactDto> SaveClientContactAsync(CRMClientContactDto request);
        Task<CRMClientContactDto> GetClientContactByClientContactIdAsync(long clientContactId);

        Task<SuccessResponse> DeleteClientContact(long clientContactId);

        Task<CRMClientContactSectionDto> GetAllClientContactsByClientId(long clientId);
        #endregion

        #region Client Documents
        Task<CRMClientDocumentDto> CreateClientDocumentAsync();
        Task<CRMClientDocumentSectionDto> GetClientDocumentsByClientIdAsync(long clientId);
        Task<CRMClientDocumentDto> SaveClientDocumentAsync(CRMClientDocumentDto request);
        Task<SuccessResponse> DeleteClientDocumentAsync(long clientDocumentId);
        #endregion

        #region Client Service
        Task<CRMClientServiceSearchDto> GetClientServiceSearchAsync();
        Task<SearchResult<CRMClientServiceSearchResultsDto>> SearchCRMClientServiceAsync(CRMClientServiceSearchDto request);
        Task<DDLData> GetClientServiceInitialDataAsync();
        Task<CRMClientServiceDto> CreateClientServiceAsync();
        Task<CRMClientServiceDto> SaveClientServiceAsync(CRMClientServiceDto request);
        Task<CRMClientServiceDto> GetClientServiceByClientServiceIdAsync(long clientServiceId);
        Task<CRMClientServiceSectionDto> GetAllClientServiceByClientId(long clientId);

        #endregion

        #region Legal Officer
        Task<LegalOfficerSearchDto> GetLegalOfficerSearchAsync();
        Task<SearchResult<LegalOfficerSearchResultsDto>> SearchLegalOfficerAsync(LegalOfficerSearchDto request);
        Task<DDLData> GetLegalOfficerInitialDataAsync();
        Task<LegalOfficerDto> CreateLegalOfficerAsync();
        Task<LegalOfficerDto> SaveLegalOfficerAsync(LegalOfficerDto request);
        Task<LegalOfficerDto> GetLegalOfficerByLegalOfficerIdAsync(long LegalOfficerId);
        Task<SuccessResponse> DeleteLegalOfficer(long LegalOfficerId);
        Task<long> GetLegalOfficerIdbyUserLoginId(string userLoginId);

        Task<List<LegalOfficerSchedulesDto>> LoadLegalOfficerSchedule(long LegalOfficerId);
        Task<List<LegalOfficerSchedulesDto>> SaveLegalOfficerSchedules(LegalOfficerSchedulesDto request);
        Task<LegalOfficerBlockedDatesDto> CreateLegalOfficerBlockDate();
        Task<List<LegalOfficerBlockedDatesDto>> SaveLegalOfficerBlockDate(LegalOfficerBlockedDatesDto Request);
        #endregion

        #region Legal Officer Appointment

        Task<LegalOfficerAppoinmentDto> CreateLegalOfficerAppoinmentAsync();
        Task<LegalOfficerAppoinmentDto> SaveLegalOfficerAppoinmentAsync(LegalOfficerAppoinmentDto request);
        Task<List<AppoinmentCalendarDto>> GetAppoinmentCalendarAsync(long legalOfficerId, int month, int year);
        Task<List<AppoinmentTimeSlotsDto>> GetAppoinmentTimeSlotsByDateAsync(long legalOfficerId, string date);

        #endregion

        #region Legal Officer Schedules

        Task<LegalOfficerSchedulesDto> CreateLegalOfficerScheduleAsync();
        //Task<LegalOfficerSchedulesDto> SaveLegalOfficerScheduleAsync(LegalOfficerSchedulesDto request);
        Task<LegalOfficerSchedulesDto> LoadSlotPreview(LegalOfficerSchedulesDto request);

        #endregion

        #region Legal Officer Blocked Dates
        Task<LegalOfficerBlockedDateSearchDto> GetLegalOfficerBlockedDatesSearchAsync();
        Task<SearchResult<LegalOfficerBlockedDateSearchResultsDto>> SearchLegalOfficerBlockedDatesAsync(LegalOfficerBlockedDateSearchDto request);
        Task<LegalOfficerBlockedDatesDto> GetLegalOfficerBlockedDateByLegalOfficerBlockDateIdAsync(long LegalOfficerBlockDateId);
        #endregion
    }
}
