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
        public async Task<IActionResult> SaveClient(CRMClientDto request)
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

        [HttpGet("GetAllClientDocumentsByClientId")]
        [SwaggerOperation(Tags = new[] { "CRMClientDocuments" }, Summary = "GetAllClientDocumentsByClientId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientDocumentSectionDto))]
        public async Task<IActionResult> GetAllClientDocumentsByClientIdAsync([FromQuery] long clientId)
        {
            return this.Ok(await _iCRMClientService.GetClientDocumentsByClientIdAsync(clientId));
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
        #endregion

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

        #region Legal Officer

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
        [AllowAnonymous]
        [HttpGet("GetLegalOfficerIdbyUserLoginId")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "GetLegalOfficerIdbyUserLoginId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        public async Task<IActionResult> GetLegalOfficerIdbyUserLoginId([FromQuery] string UserLoginId)
        {
            return this.Ok(await _iCRMClientService.GetLegalOfficerIdbyUserLoginId(UserLoginId));
        }
        #endregion

        #region Load LegalOfficer Schedule
        [AllowAnonymous]
        [HttpGet("LoadLegalOfficerSchedule")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "LoadLegalOfficerSchedule")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LegalOfficerSchedulesDto>))]
        public async Task<IActionResult> LoadLegalOfficerSchedule([FromQuery] long LegalOfficerId)
        {
            return this.Ok(await _iCRMClientService.LoadLegalOfficerSchedule(LegalOfficerId));
        }
        #endregion
        #region Save LegalOfficer Schedule
        [AllowAnonymous]
        [HttpPost("SaveLegalOfficerSchedule")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "SaveLegalOfficerSchedule")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LegalOfficerSchedulesDto>))]
        public async Task<IActionResult> SaveLegalOfficerSchedule(LegalOfficerSchedulesDto Request)
        {
            return this.Ok(await _iCRMClientService.SaveLegalOfficerSchedules(Request));
        }
        #endregion

        #region Create LegalOfficer BlockDate 
        [AllowAnonymous]
        [HttpGet("CreateLegalOfficerBlockDate")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "CreateLegalOfficerBlockDate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LegalOfficerBlockedDatesDto))]
        public async Task<IActionResult> CreateLegalOfficerBlockDate()
        {
            return this.Ok(await _iCRMClientService.CreateLegalOfficerBlockDate());
        }
        #endregion

        #region Save LegalOfficer BlockDate
        [AllowAnonymous]
        [HttpPost("SaveLegalOfficerBlockDate")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "SaveLegalOfficerBlockDate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LegalOfficerBlockedDatesDto>))]
        public async Task<IActionResult> SaveLegalOfficerBlockDate(LegalOfficerBlockedDatesDto Request)
        {
            return this.Ok(await _iCRMClientService.SaveLegalOfficerBlockDate(Request));
        }
        #endregion

        #endregion

    }
}

