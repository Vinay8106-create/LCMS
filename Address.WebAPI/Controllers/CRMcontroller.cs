using CRM.Application;
using Galaxy.Dto;
using LCMS.Dto;
using LCMS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CRM.WebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CRMController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IDocumentFileService _DocumentFileService;
        private readonly ICRMClientService _iCRMClientService;

        public CRMController(IAddressService addressService, IDocumentFileService DocumentFileService, ICRMClientService iCRMClientService)
        {
            _addressService = addressService;

            _DocumentFileService = DocumentFileService;
            _iCRMClientService = iCRMClientService;
        }


        #region Client Search

        [HttpGet("GetClientSearch")]
        [SwaggerOperation(Tags = new[] { "CRMClient" }, Summary = "GetClientSearch")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientSearchDto))]
        public async Task<IActionResult> GetClientSearch()
        {
            return this.Ok(await _iCRMClientService.GetClientSearchAsync());
        }

        [HttpPost("SearchCRMClient")]
        [SwaggerOperation(Tags = new[] { "CRMClient" }, Summary = "SearchCRMClient")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<CRMClientSearchResultsDto>))]
        public async Task<IActionResult> SearchCRMClient(CRMClientSearchDto request)
        {
            return this.Ok(await _iCRMClientService.SearchCRMClient(request));
        }
        #endregion

        #region Client

        [HttpGet("GetClientInitialData")]
        [SwaggerOperation(Tags = new[] { "CRMClient" }, Summary = "GetClientInitialData")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DDLData))]
        public async Task<IActionResult> GetClientInitialData()
        {
            return this.Ok(await _iCRMClientService.GetClientInitialDataAsync());
        }


        [HttpGet("CreateClient")]
        [SwaggerOperation(Tags = new[] { "CRMClient" }, Summary = "CreateClient")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientDto))]
        public async Task<IActionResult> CreateClient()
        {
            return this.Ok(await _iCRMClientService.CreateClientAsync());
        }

        [HttpPost("SaveClient")]
        [SwaggerOperation(Tags = new[] { "CRMClient" }, Summary = "SaveClient")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
        public async Task<IActionResult> SaveClient([FromBody] CRMClientDto request)
        {
            return this.Ok(await _iCRMClientService.SaveClientAsync(request));
        }

        [HttpGet("GetClientByClientId")]
        [SwaggerOperation(Tags = new[] { "CRMClient" }, Summary = "GetClientByClientId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientDto))]
        public async Task<IActionResult> GetClientByClientId([FromQuery] long clientId)
        {
            return this.Ok(await _iCRMClientService.GetClientByClientIdAsync(clientId));
        }

        [HttpGet("GetAppointmentsByClientId")]
        [SwaggerOperation(Tags = new[] { "CRMClient" }, Summary = "GetAppointmentsByClientId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LegalOfficerAppoinmentDto>))]
        public async Task<IActionResult> GetAppointmentsByClientId([FromQuery] long clientId)
        {
            return this.Ok(await _iCRMClientService.GetAppointmentsByClientIdAsync(clientId));
        }
        #endregion

        #region Client Contact 

        #region Create Contact 
        [HttpGet("CreateClientContact")]
        [SwaggerOperation(Tags = new[] { "CRMClientContact" }, Summary = "CreateClientContact")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientContactDto))]
        public async Task<IActionResult> CreateClientContact()
        {
            return this.Ok(await _iCRMClientService.CreateClientContactAsync());
        }
        #endregion

        #region Save Contact 
        [HttpPost("SaveClientContact")]
        [SwaggerOperation(Tags = new[] { "CRMClientContact" }, Summary = "SaveClientContact")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientContactDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
        public async Task<IActionResult> SaveClientContact(CRMClientContactDto request)
        {
            return this.Ok(await _iCRMClientService.SaveClientContactAsync(request));
        }
        #endregion

        #region GetClientContactByClientContactIdAsync

        [HttpGet("GetClientContactByClientContactId")]
        [SwaggerOperation(Tags = new[] { "CRMClientContact" }, Summary = "GetClientContactByClientContactId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientContactDto))]
        public async Task<IActionResult> GetClientContactByClientContactIdAsync([FromQuery] long clientContactId)
        {
            return this.Ok(await _iCRMClientService.GetClientContactByClientContactIdAsync(clientContactId));
        }
        #endregion

        #region Delete Client Contact

        [HttpDelete("DeleteClientContact")]
        [SwaggerOperation(Tags = new[] { "CRMClientContact" }, Summary = "DeleteClientContact")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse))]
        public async Task<IActionResult> DeleteClientContact([FromQuery] long clientContactId)
        {
            return this.Ok(await _iCRMClientService.DeleteClientContact(clientContactId));
        }

        #endregion

        #region Get All Client Contacts By ClientId
        [HttpGet("GetAllClientContactsByClientId")]
        [SwaggerOperation(Tags = new[] { "CRMClientContact" }, Summary = "GetAllClientContactsByClientId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientContactSectionDto))]
        public async Task<IActionResult> GetAllClientContactsByClientIdAsync([FromQuery] long clientId)
        {
            return this.Ok(await _iCRMClientService.GetAllClientContactsByClientId(clientId));
        }
        #endregion

        #endregion

        #region CRMClient Documents

        #region Create Client Documents
        [HttpGet("CreateClientDocument")]
        [SwaggerOperation(Tags = new[] { "CRMClientDocument" }, Summary = "CreateClientDocument")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientDocumentDto))]
        public async Task<IActionResult> CreateClientDocument()
        {
            return this.Ok(await _iCRMClientService.CreateClientDocumentAsync());
        }
        #endregion

        #region Save Client Document 
        [HttpPost("SaveClientDocument")]
        [SwaggerOperation(Tags = new[] { "CRMClientDocument" }, Summary = "SaveClientDocument")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientDocumentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
        public async Task<IActionResult> SaveClientDocument(CRMClientDocumentDto request)
        {
            return this.Ok(await _iCRMClientService.SaveClientDocumentAsync(request));
        }
        #endregion

        [HttpGet("GetAllClientDocumentsByClientId")]
        [SwaggerOperation(Tags = new[] { "CRMClientDocument" }, Summary = "GetAllClientDocumentsByClientId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientDocumentSectionDto))]
        public async Task<IActionResult> GetAllClientDocumentsByClientIdAsync([FromQuery] long clientId)
        {
            return this.Ok(await _iCRMClientService.GetClientDocumentsByClientIdAsync(clientId));
        }

        [HttpDelete("DeleteClientDocument")]
        [SwaggerOperation(Tags = new[] { "CRMClientDocument" }, Summary = "DeleteClientDocument")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse))]
        public async Task<IActionResult> DeleteClientDocument([FromQuery] long clientDocumentId)
        {
            return this.Ok(await _iCRMClientService.DeleteClientDocumentAsync(clientDocumentId));
        }

        #endregion

        #region Document File

        [HttpPost("GetDocumentFile")]
        [SwaggerOperation(Tags = new[] { "DocumentFile" }, Summary = "GetDocumentFile")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentFileDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
        public async Task<IActionResult> GetDocumentFile(long documentFileId)
        {
            return this.Ok(await _DocumentFileService.GetDocumentFile(documentFileId));
        }


        [HttpPost("SaveDocument")]
        [SwaggerOperation(Tags = new[] { "DocumentFile" }, Summary = "SaveDocument")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentFileDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
        public async Task<IActionResult> SaveDocumentFile(DocumentFileDto request)
        {
            return this.Ok(await _DocumentFileService.SaveDocumentFile(request));
        }


        [HttpPost("DeleteDocument")]
        [SwaggerOperation(Tags = new[] { "DocumentFile" }, Summary = "DeleteDocument")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentFileDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
        public async Task<IActionResult> DeleteDocumentFile(long documentFileId)
        {
            return this.Ok(await _DocumentFileService.DeleteDocumentFile(documentFileId));
        }

        #endregion

        #region Client Service Search

        [HttpGet("GetClientServiceSearch")]
        [SwaggerOperation(Tags = new[] { "CRMClientService" }, Summary = "GetClientServiceSearch")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientServiceSearchDto))]
        public async Task<IActionResult> GetClientServiceSearch()
        {
            return this.Ok(await _iCRMClientService.GetClientServiceSearchAsync());
        }

        [HttpPost("SearchCRMClientService")]
        [SwaggerOperation(Tags = new[] { "CRMClientService" }, Summary = "SearchCRMClientService")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<CRMClientServiceSearchResultsDto>))]
        public async Task<IActionResult> SearchCRMClientService(CRMClientServiceSearchDto request)
        {
            return this.Ok(await _iCRMClientService.SearchCRMClientServiceAsync(request));
        }
        #endregion

        #region Client Service
        [HttpGet("GetClientServiceInitialData")]
        [SwaggerOperation(Tags = new[] { "CRMClientService" }, Summary = "GetClientServiceInitialData")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DDLData))]
        public async Task<IActionResult> GetClientServiceInitialData()
        {
            return this.Ok(await _iCRMClientService.GetClientServiceInitialDataAsync());
        }


        [HttpGet("CreateClientService")]
        [SwaggerOperation(Tags = new[] { "CRMClientService" }, Summary = "CreateClientService")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientServiceDto))]
        public async Task<IActionResult> CreateClientService()
        {
            return this.Ok(await _iCRMClientService.CreateClientServiceAsync());
        }

        [HttpPost("SaveClientService")]
        [SwaggerOperation(Tags = new[] { "CRMClientService" }, Summary = "SaveClientService")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientServiceDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
        public async Task<IActionResult> SaveClientService(CRMClientServiceDto request)
        {
            return this.Ok(await _iCRMClientService.SaveClientServiceAsync(request));
        }

        #region GetClientServiceByClientServiceIdAsync

        [HttpGet("GetClientServiceByClientServiceIdAsync")]
        [SwaggerOperation(Tags = new[] { "CRMClientService" }, Summary = "GetClientServiceByClientServiceIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientServiceDto))]
        public async Task<IActionResult> GetClientServiceByClientServiceIdAsync([FromQuery] long clientServiceId)
        {
            return this.Ok(await _iCRMClientService.GetClientServiceByClientServiceIdAsync(clientServiceId));
        }
        #endregion

        #region GetClientServiceStatusHistoryByClientServiceIdAsync

        [HttpGet("GetClientServiceStatusHistoryByClientServiceIdAsync")]
        [SwaggerOperation(Tags = new[] { "CRMClientService" }, Summary = "GetClientServiceStatusHistoryByClientServiceIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CRMClientServiceStatusHistoryDto>))]
        public async Task<IActionResult> GetClientServiceStatusHistoryByClientServiceIdAsync([FromQuery] long clientServiceId)
        {
            return this.Ok(await _iCRMClientService.GetClientServiceStatusHistoryByClientServiceIdAsync(clientServiceId));
        }
        #endregion

        #region GetClientServiceEmailHistoryByClientServiceIdAsync

        [HttpGet("GetClientServiceEmailHistoryByClientServiceIdAsync")]
        [SwaggerOperation(Tags = new[] { "CRMClientService" }, Summary = "GetClientServiceEmailHistoryByClientServiceIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CRMClientServiceEmailHistoryDto>))]
        public async Task<IActionResult> GetClientServiceEmailHistoryByClientServiceIdAsync([FromQuery] long clientServiceId)
        {
            return this.Ok(await _iCRMClientService.GetClientServiceEmailHistoryByClientServiceIdAsync(clientServiceId));
        }
        #endregion

        #region GetClientServiceAssignedOfficerHistoryByClientServiceIdAsync

        [HttpGet("GetClientServiceAssignedOfficerHistoryByClientServiceIdAsync")]
        [SwaggerOperation(Tags = new[] { "CRMClientService" }, Summary = "GetClientServiceAssignedOfficerHistoryByClientServiceIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CRMClientServiceAssignedOfficerHistoryDto>))]
        public async Task<IActionResult> GetClientServiceAssignedOfficerHistoryByClientServiceIdAsync([FromQuery] long clientServiceId)
        {
            return this.Ok(await _iCRMClientService.GetClientServiceAssignedOfficerHistoryByClientServiceIdAsync(clientServiceId));
        }
        #endregion

        #region GetClientServiceNotesByClientServiceIdAsync

        [HttpGet("GetClientServiceNotesByClientServiceIdAsync")]
        [SwaggerOperation(Tags = new[] { "CRMClientService" }, Summary = "GetClientServiceNotesByClientServiceIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CRMClientServiceNotesDto>))]
        public async Task<IActionResult> GetClientServiceNotesByClientServiceIdAsync([FromQuery] long clientServiceId)
        {
            return this.Ok(await _iCRMClientService.GetClientServiceNotesByClientServiceIdAsync(clientServiceId));
        }
        #endregion

        #region Get All Client Services By ClientId
        [HttpGet("GetAllClientServicesByClientId")]
        [SwaggerOperation(Tags = new[] { "CRMClientService" }, Summary = "GetAllClientServicesByClientId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientServiceSectionDto))]
        public async Task<IActionResult> GetAllClientServicesByClientId([FromQuery] long clientId)
        {
            return this.Ok(await _iCRMClientService.GetAllClientServiceByClientId(clientId));
        }
        #endregion

        #endregion

        #region Legal Officer

        #region Legal Officer Search

        [HttpGet("GetLegalOfficerSearch")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "GetLegalOfficerSearch")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LegalOfficerSearchDto))]
        public async Task<IActionResult> GetLegalOfficerSearch()
        {
            return this.Ok(await _iCRMClientService.GetLegalOfficerSearchAsync());
        }

        [HttpPost("SearchLegalOfficer")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "SearchLegalOfficer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<LegalOfficerSearchResultsDto>))]
        public async Task<IActionResult> SearchLegalOfficer(LegalOfficerSearchDto request)
        {
            return this.Ok(await _iCRMClientService.SearchLegalOfficerAsync(request));
        }
        #endregion

        #region GetLegalOfficerInitialData
        [HttpGet("GetLegalOfficerInitialData")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "GetLegalOfficerInitialData")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DDLData))]
        public async Task<IActionResult> GetLegalOfficerInitialData()
        {
            return this.Ok(await _iCRMClientService.GetLegalOfficerInitialDataAsync());
        }
        #endregion

        #region Create LegalOfficer 
        [HttpGet("CreateLegalOfficer")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "CreateLegalOfficer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LegalOfficerDto))]
        public async Task<IActionResult> CreateLegalOfficer()
        {
            return this.Ok(await _iCRMClientService.CreateLegalOfficerAsync());
        }
        #endregion

        #region Save LegalOfficer 
        [AllowAnonymous]
        [HttpPost("SaveLegalOfficer")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "SaveLegalOfficer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LegalOfficerDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
        public async Task<IActionResult> SaveLegalOfficer(LegalOfficerDto request)
        {
            return this.Ok(await _iCRMClientService.SaveLegalOfficerAsync(request));
        }
        #endregion

        #region Get LegalOfficer 
        [HttpGet("GetLegalOfficerByLegalOfficerId")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "GetLegalOfficerByLegalOfficerId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LegalOfficerDto))]
        public async Task<IActionResult> GetLegalOfficerByLegalOfficerIdAsync([FromQuery] long LegalOfficerId)
        {
            return this.Ok(await _iCRMClientService.GetLegalOfficerByLegalOfficerIdAsync(LegalOfficerId));
        }
        #endregion

        #region Delete LegalOfficer 
        [HttpDelete("DeleteLegalOfficer")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "DeleteLegalOfficer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse))]
        public async Task<IActionResult> DeleteLegalOfficer([FromQuery] long LegalOfficerId)
        {
            return this.Ok(await _iCRMClientService.DeleteLegalOfficer(LegalOfficerId));
        }
        #endregion

        #region GetLegalOfficerIdbyUserLoginId
        [HttpGet("GetLegalOfficerIdbyUserLoginId")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "GetLegalOfficerIdbyUserLoginId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        public async Task<IActionResult> GetLegalOfficerIdbyUserLoginId([FromQuery] string UserLoginId)
        {
            return this.Ok(await _iCRMClientService.GetLegalOfficerIdbyUserLoginId(UserLoginId));
        }
        #endregion

        #endregion

        #region Legal Officer Schedule

        #region Load LegalOfficer Schedule
        [HttpGet("LoadLegalOfficerSchedule")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "LoadLegalOfficerSchedule")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LegalOfficerSchedulesDto>))]
        public async Task<IActionResult> LoadLegalOfficerSchedule([FromQuery] long LegalOfficerId)
        {
            return this.Ok(await _iCRMClientService.LoadLegalOfficerSchedule(LegalOfficerId));
        }
        #endregion

        #region Save LegalOfficer Schedule
        [HttpPost("SaveLegalOfficerSchedule")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "SaveLegalOfficerSchedule")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LegalOfficerSchedulesDto>))]
        public async Task<IActionResult> SaveLegalOfficerSchedule(LegalOfficerSchedulesDto Request)
        {
            return this.Ok(await _iCRMClientService.SaveLegalOfficerSchedules(Request));
        }
        #endregion

        #endregion

        #region Legal Officer Appointment

        #region GetLegalOfficerAppoinmentInitialData
        [HttpGet("GetLegalOfficerAppoinmentInitialData")]
        [SwaggerOperation(Tags = new[] { "LegalOfficerAppoinment" }, Summary = "GetLegalOfficerAppoinmentInitialData")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DDLData))]
        public async Task<IActionResult> GetLegalOfficerAppoinmentInitialData()
        {
            return this.Ok(await _iCRMClientService.GetLegalOfficerAppoinmentInitialDataAsync());
        }
        #endregion

        #region Legal Officer Appointment Search

        [HttpGet("GetLegalOfficerAppoinmentSearch")]
        [SwaggerOperation(Tags = new[] { "LegalOfficerAppoinment" }, Summary = "GetLegalOfficerAppoinmentSearch")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LegalOfficerAppoinmentSearchDto))]
        public async Task<IActionResult> GetLegalOfficerAppoinmentSearch()
        {
            return this.Ok(await _iCRMClientService.GetLegalOfficerAppoinmentSearchAsync());
        }

        [HttpPost("SearchLegalOfficerAppoinment")]
        [SwaggerOperation(Tags = new[] { "LegalOfficerAppoinment" }, Summary = "SearchLegalOfficerAppoinment")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<LegalOfficerAppoinmentSearchResultsDto>))]
        public async Task<IActionResult> SearchLegalOfficerAppoinment(LegalOfficerAppoinmentSearchDto request)
        {
            return this.Ok(await _iCRMClientService.SearchLegalOfficerAppoinmentAsync(request));
        }
        #endregion

        #region Create Legal Officer Appointment
        [HttpGet("CreateLegalOfficerAppointment")]
        [SwaggerOperation(Tags = new[] { "LegalOfficerAppoinment" }, Summary = "CreateLegalOfficerAppointment")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LegalOfficerAppoinmentDto))]
        public async Task<IActionResult> CreateLegalOfficerAppointment()
        {
            return this.Ok(await _iCRMClientService.CreateLegalOfficerAppoinmentAsync());
        }
        #endregion

        #region Save Legal Officer Appoinment
        [HttpPost("SaveLegalOfficerAppoinment")]
        [SwaggerOperation(Tags = new[] { "LegalOfficerAppoinment" }, Summary = "SaveLegalOfficerAppoinment")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LegalOfficerAppoinmentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
        public async Task<IActionResult> SaveLegalOfficerAppoinment(LegalOfficerAppoinmentDto request)
        {
            return this.Ok(await _iCRMClientService.SaveLegalOfficerAppoinmentAsync(request));
        }
        #endregion

        #region GetAppoinment Calender
        [HttpGet("GetAppoinmentCalendar")]
        [SwaggerOperation(Tags = new[] { "LegalOfficerAppoinment" }, Summary = "GetAppoinmentCalendar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AppoinmentCalendarDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
        public async Task<IActionResult> GetAppoinmentCalendar([FromQuery] long legalOfficerId,
                                 [FromQuery] int month,
                                 [FromQuery] int year)
        {
            return this.Ok(await _iCRMClientService.GetAppoinmentCalendarAsync(legalOfficerId, month, year));
        }
        #endregion


        #region GetAppoinment Time Slots By Date
        [HttpGet("GetAppoinmentTimeSlotsByDate")]
        [SwaggerOperation(Tags = new[] { "LegalOfficerAppoinment" }, Summary = "GetAppoinmentTimeSlotsByDate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AppoinmentTimeSlotsDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
        public async Task<IActionResult> GetAppoinmentTimeSlotsByDate([FromQuery] long legalOfficerId, [FromQuery] string date)
        {
            return this.Ok(await _iCRMClientService.GetAppoinmentTimeSlotsByDateAsync(legalOfficerId, date));
        }
        #endregion

        #region Load Slot Preview
        [HttpPost("LoadSlotPreview")]
        [SwaggerOperation(Tags = new[] { "LegalOfficerAppoinment" }, Summary = "LoadSlotPreview")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LegalOfficerSchedulesDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
        public async Task<IActionResult> LoadSlotPreview([FromBody] LegalOfficerSchedulesDto request)
        {
            return this.Ok(await _iCRMClientService.LoadSlotPreview(request));
        }
        #endregion

        #endregion

        #region Legal Officer Blocked Dates

        #region Legal Officer Blocked Dates Search

        [HttpGet("GetLegalOfficerBlockedDatesSearch")]
        [SwaggerOperation(Tags = new[] { "LegalOfficerBlockedDates" }, Summary = "GetLegalOfficerBlockedDatesSearch")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LegalOfficerBlockedDateSearchDto))]
        public async Task<IActionResult> GetLegalOfficerBlockedDatesSearch()
        {
            return this.Ok(await _iCRMClientService.GetLegalOfficerBlockedDatesSearchAsync());
        }

        [HttpPost("SearchLegalOfficerBlockedDates")]
        [SwaggerOperation(Tags = new[] { "LegalOfficerBlockedDates" }, Summary = "SearchLegalOfficerBlockedDates")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<LegalOfficerBlockedDateSearchResultsDto>))]
        public async Task<IActionResult> SearchLegalOfficerBlockedDates(LegalOfficerBlockedDateSearchDto request)
        {
            return this.Ok(await _iCRMClientService.SearchLegalOfficerBlockedDatesAsync(request));
        }
        #endregion

        #region Create LegalOfficer BlockDate 
        [HttpGet("CreateLegalOfficerBlockDate")]
        [SwaggerOperation(Tags = new[] { "LegalOfficerBlockedDates" }, Summary = "CreateLegalOfficerBlockDate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LegalOfficerBlockedDatesDto))]
        public async Task<IActionResult> CreateLegalOfficerBlockDate()
        {
            return this.Ok(await _iCRMClientService.CreateLegalOfficerBlockDate());
        }
        #endregion

        #region Save LegalOfficer BlockDate
        [HttpPost("SaveLegalOfficerBlockDate")]
        [SwaggerOperation(Tags = new[] { "LegalOfficerBlockedDates" }, Summary = "SaveLegalOfficerBlockDate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LegalOfficerBlockedDatesDto>))]
        public async Task<IActionResult> SaveLegalOfficerBlockDate(LegalOfficerBlockedDatesDto Request)
        {
            return this.Ok(await _iCRMClientService.SaveLegalOfficerBlockDate(Request));
        }
        #endregion

        #region Get Legal Officer Blocked Date
        [HttpGet("GetLegalOfficerBlockedDateByLegalOfficerBlockedDateId")]
        [SwaggerOperation(Tags = new[] { "LegalOfficerBlockedDates" }, Summary = "GetLegalOfficerBlockedDateByLegalOfficerBlockedDateId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LegalOfficerBlockedDatesDto))]
        public async Task<IActionResult> GetLegalOfficerBlockedDateByLegalOfficerBlockedDateId([FromQuery] long LegalOfficerBlockedDateId)
        {
            return this.Ok(await _iCRMClientService.GetLegalOfficerBlockedDateByLegalOfficerBlockDateIdAsync(LegalOfficerBlockedDateId));
        }
        #endregion

        #endregion

    }
}

