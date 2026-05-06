using CRM.Application;
using Galaxy.Dto;
using LCMS.Dto;
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

        [HttpGet("GetClientContactByClientContactId")]
        [SwaggerOperation(Tags = new[] { "CRMClientContact" }, Summary = "GetClientContactByClientContactId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CRMClientDto))]
        public async Task<IActionResult> GetClientByClientContactIdAsync([FromQuery] long clientId)
        {
            return this.Ok(await _iCRMClientService.GetClientContactByClientContactIdAsync(clientId));
        }

        [HttpDelete("DeleteClientContact")]
        [SwaggerOperation(Tags = new[] { "CRMClientContact" }, Summary = "DeleteClientContact")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse))]
        public async Task<IActionResult> DeleteClientContact([FromQuery] long clientContactId)
        {
            return this.Ok(await _iCRMClientService.DeleteClientContact(clientContactId));
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

        #region Legal Officer

        [HttpGet("GetLegalOfficerInitialData")]
        [SwaggerOperation(Tags = new[] { "LegalOfficer" }, Summary = "GetLegalOfficerInitialData")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DDLData))]
        public async Task<IActionResult> GetLegalOfficerInitialData()
        {
            return this.Ok(await _iCRMClientService.GetLegalOfficerInitialDataAsync());
        }
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
        public async Task<IActionResult> GetLegalOfficerByLegalOfficerIdAsync([FromQuery] long clientId)
        {
            return this.Ok(await _iCRMClientService.GetLegalOfficerByLegalOfficerIdAsync(clientId));
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

        #endregion

    }
}

