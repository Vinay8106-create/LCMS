using AutoMapper;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Domain.Models;
using Galaxy.Dto;
using LCMS.Constants;
using LCMS.Dto;
using LCMS.DTO;
using LCMS.S2SLogic;
using System.Net;

namespace CRM.Application
{
    public class CRMClientServices : ICRMClientService
    {
        private readonly ICRMUow _iCRMUow;
        private readonly IMapper _mapper;
        private readonly IAddressService _iAddressService;
        private readonly IDocumentFileService _iDocumentService;
        protected readonly BaseRequestProfile _UserProfile;
        protected readonly IS2SLogic _s2SLogic;

        public CRMClientServices(ICRMUow iCRMUow, IMapper mapper, IAddressService addressService,
            IDocumentFileService DocumentService, BaseRequestProfile userProfile, IS2SLogic s2SLogic)
        {
            _iCRMUow = iCRMUow;
            _mapper = mapper;
            _iAddressService = addressService;
            _iDocumentService = DocumentService;
            _UserProfile = userProfile;
            _s2SLogic = s2SLogic;
        }

        #region CRM Client

        #region Get Client Initial Data
        public async Task<DDLData> GetClientInitialDataAsync()
        {
            return await _iCRMUow.ConfigRepo.ClientInitialData();
        }
        #endregion

        #region CRMClient Search
        public virtual async Task<CRMClientSearchDto> GetClientSearchAsync()
        {
            return new CRMClientSearchDto();
        }

        public virtual async Task<SearchResult<CRMClientSearchResultsDto>> SearchCRMClient(CRMClientSearchDto request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            if (string.IsNullOrEmpty(request.OrderByColumnName))
                request.OrderByColumnName = nameof(CRMClientSearchResultsDto.ClientId);

            var result = await _iCRMUow.CRMClientSearchRepo.SearchAsync(request);
            if (result != null)
            {
                result.Msg ??= new AppMessage();
                result.Msg.InfoMessage = result.TotalCount > 0
                    ? _mapper.Map(await _iCRMUow.MessageRepo.GetMessageByNo(4, result.TotalCount), result.Msg.InfoMessage)
                    : _mapper.Map(await _iCRMUow.MessageRepo.GetMessageByNo(3), result.Msg.InfoMessage);
                return result;
            }
            return new SearchResult<CRMClientSearchResultsDto>();
        }
        #endregion

        #region Create Client
        public async Task<CRMClientDto> CreateClientAsync()
        {
            return new CRMClientDto
            {
                ClientTypeConfigId = 1,
            };
        }
        #endregion

        #region Save Client
        public async Task<CRMClientDto> SaveClientAsync(CRMClientDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var client = _mapper.Map<CRMClient>(request);
            string groupName = CRMConstants.GroupName.CSO;
            var validUser = await _s2SLogic.Admin
                                 .IsUserBasedOnConfiguredGroup(_UserProfile.CurrentUser, groupName);
            if (validUser != true)
                throw new BusinessException(
                    await _iCRMUow.MessageRepo.GetMessageByNo(1001), HttpStatusCode.BadRequest);

            client.ValidateMandatoryFields();
            var residentialAddress = _mapper.Map<Address>(request.ResidentialAddress);
            residentialAddress.ValidateMandatoryFields();
            var communicationAddress = _mapper.Map<Address>(request.CommunicationAddress);
            communicationAddress.ValidateMandatoryFields();

            if (client.HasError)
                throw new BusinessException(
                    client.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);
            if (residentialAddress.HasError)
                throw new BusinessException(
                    residentialAddress.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);
            if (communicationAddress.HasError)
                throw new BusinessException(
                    communicationAddress.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);


            var photo = await _iDocumentService.SaveDocumentFile(request.Photo);
            if (photo.Id > 0)
                client.Photo = null;

            if (string.IsNullOrWhiteSpace(client.RefNo))
                await GenerateClientNo(client);


            client.ResidentialAddress = residentialAddress;
            client.CommunicationAddress = communicationAddress;
            client.PhotoId = photo.Id;


            if (client.Id > 0)
                await _iCRMUow.CRMClientRepo.UpdateClient(request);
            else
                await _iCRMUow.CRMClientRepo.InsertClient(client);

            await _iCRMUow.SaveChangesAsync();

            var response = _mapper.Map<CRMClientDto>(client);
            response.Photo = photo;
            await SetDescription(response);
            return response;
        }

        private async Task<CRMClient> GenerateClientNo(CRMClient client)
        {
            await _iCRMUow.CRMClientRepo.GenerateClientRefNo(client);
            if (string.IsNullOrWhiteSpace(client.RefNo))
            {
                throw new BusinessException(await _iCRMUow.MessageRepo.GetMessageByNo(CRMConstants.CRMClientReferenceNumber.ErrorGeneratingRefNo, "Client Ref Number"), HttpStatusCode.BadRequest);
            }

            return client;
        }
        #endregion

        #region Set Descriptions

        private async Task SetDescription(CRMClientDto client)
        {
            await _iCRMUow.ConfigRepo.SetDescription(client);
        }

        private async Task SetDescription(CRMClientContactDto clientContact)
        {
            await _iCRMUow.ConfigRepo.SetDescription(clientContact);
        }

        private async Task SetDescription(CRMClientServiceDto clientService)
        {
            await _iCRMUow.ConfigRepo.SetDescription(clientService);
        }

        private async Task SetDescription(LegalOfficerDto legalOfficerDto)
        {
            await _iCRMUow.ConfigRepo.SetDescription(legalOfficerDto);
        }

        private async Task SetDescription(LegalOfficerAppoinmentDto legalOfficerAppoinment)
        {
            await _iCRMUow.ConfigRepo.SetDescription(legalOfficerAppoinment);
        }

        private async Task SetDescription(LegalOfficerBlockedDatesDto legalOfficerBlockedDate)
        {
            await _iCRMUow.ConfigRepo.SetDescription(legalOfficerBlockedDate);
        }

        private async Task SetDescription(AppoinmentTimeSlotsDto appoinmentTimeSlotsDto)
        {
            await _iCRMUow.ConfigRepo.SetDescription(appoinmentTimeSlotsDto);
        }

        #endregion

        #region Get Client By Client Id

        public async Task<CRMClientDto> GetClientByClientIdAsync(long clientId)
        {
            var client = await _iCRMUow.CRMClientRepo.GetClientById(clientId);

            if (client == null) return null;

            var clientDto = _mapper.Map<CRMClientDto>(client);

            await _iCRMUow.ConfigRepo.SetDescription(clientDto);
            await _iCRMUow.ConfigRepo.SetAddressDescription(clientDto.ResidentialAddress);
            await _iCRMUow.ConfigRepo.SetAddressDescription(clientDto.CommunicationAddress);

            if (clientDto.Photo != null)
                clientDto.Photo.base64FileContent = await GetBase64FromFile(
                    clientDto.Photo.RelativePath,
                    clientDto.Photo.FileName
                );

            return clientDto;
        }

        private async Task<string?> GetBase64FromFile(string? relativePath, string? fileName)
        {
            if (string.IsNullOrEmpty(relativePath) || string.IsNullOrEmpty(fileName))
                return null;

            // Combine path: D:\Build\LCMS\build\Files + filename
            var fullPath = Path.Combine(relativePath, fileName);

            if (!File.Exists(fullPath))
                return null;

            var fileBytes = await File.ReadAllBytesAsync(fullPath);
            return Convert.ToBase64String(fileBytes);
        }
        #endregion

        #region Get Client Appointments By Client Id

        public async Task<List<LegalOfficerAppoinmentDto>> GetAppointmentsByClientIdAsync(long clientId)
        {
            List<LegalOfficerAppoinmentDto> list = new List<LegalOfficerAppoinmentDto>();
            var data = _iCRMUow.LegalOfficerAppoinmentRepo.Query(x => x.ClientId == clientId).ToList();
            if (data != null && data.Count > 0)
            {
                var client = await _iCRMUow.CRMClientRepo.GetClientById(clientId);
                if (client == null) throw new BusinessException("There is no Client With This Id, Use Valid ClientId");

                foreach (var item in data)
                {
                    item.ClientName = (client.FirstName ?? "") + " " +
                                                (client.MiddleName ?? "") + " " +
                                                (client.LastName ?? "");
                    var legalOfficer = _iCRMUow.LegalOfficerRepo.Query(x => x.Id == item.LegalOfficerId).FirstOrDefault();
                    item.LegalOfficerName = await _iCRMUow.LegalOfficerRepo.SetUserName(legalOfficer.UserSerialId);
                    var Appointment = _mapper.Map<LegalOfficerAppoinmentDto>(item);

                    await SetDescription(Appointment);

                    list.Add(Appointment);
                }
            }

            return list;
        }
        #endregion

        #endregion

        #region Client Contact

        #region Create Client Contact
        public async Task<CRMClientContactDto> CreateClientContactAsync()
        {
            return new CRMClientContactDto();
        }
        #endregion

        #region Save Client Contact
        public async Task<CRMClientContactDto> SaveClientContactAsync(CRMClientContactDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(CRMClientContactDto));
            var client = await _iCRMUow.CRMClientRepo.GetClientById(request.ClientId);
            if (client == null)
                throw new BusinessException("There Is No Client With This Id, Use Correct ClientId");

            var ClientContact = _mapper.Map<CRMClientContact>(request);
            ClientContact.ValidateMandatoryFields();

            var Residentialaddress = _mapper.Map<Address>(request.ResidentialAddress);
            Residentialaddress.ValidateMandatoryFields();

            var Communicationaddress = _mapper.Map<Address>(request.CommunicationAddress);
            Communicationaddress.ValidateMandatoryFields();

            if (ClientContact.HasError)
                throw new BusinessException(ClientContact.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);
            if (Residentialaddress.HasError)
                throw new BusinessException(Residentialaddress.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);
            if (Communicationaddress.HasError)
                throw new BusinessException(Communicationaddress.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);

            var savedResidentialAddress = await _iAddressService.SaveAddress(request.ResidentialAddress);
            var savedCommunicationAddress = await _iAddressService.SaveAddress(request.CommunicationAddress);

            ClientContact.ResidentialAddressId = savedResidentialAddress.Id;
            ClientContact.CommunicationAddressId = savedCommunicationAddress.Id;

            // Break the object reference so EF doesn't re-insert them
            ClientContact.ResidentialAddress = null;
            ClientContact.CommunicationAddress = null;

            ClientContact = ClientContact.Id > 0 ? await _iCRMUow.CRMClientContactRepo.UpdateCRMClientContactAsync(request) : await _iCRMUow.CRMClientContactRepo.InsertCRMClientContactAsync(request);

            await _iCRMUow.SaveChangesAsync();
            var response = _mapper.Map<CRMClientContactDto>(ClientContact);
            response.ResidentialAddress = _mapper.Map<AddressDto>(savedResidentialAddress);
            response.CommunicationAddress = _mapper.Map<AddressDto>(savedCommunicationAddress);
            await SetDescription(response);

            return response;
        }
        #endregion

        #region Get Client Contact By Id
        public async Task<CRMClientContactDto> GetClientContactByClientContactIdAsync(long clientContactId)
        {
            var clientcontact = _mapper.Map<CRMClientContactDto>(await _iCRMUow.CRMClientContactRepo.GetCRMClientContactByClientContactId(clientContactId));
            await _iCRMUow.ConfigRepo.SetDescription(clientcontact);
            await _iCRMUow.ConfigRepo.SetAddressDescription(clientcontact.ResidentialAddress);
            await _iCRMUow.ConfigRepo.SetAddressDescription(clientcontact.CommunicationAddress);
            return clientcontact;
        }
        #endregion

        #region Get All Client Contacts By ClientId
        public async Task<CRMClientContactSectionDto> GetAllClientContactsByClientId(long clientId)
        {
            if (clientId <= 0) throw new BusinessException("ID Is Invalid");

            return await _iCRMUow.CRMClientContactRepo.GetAllClientContactsByClientIdAsync(clientId);
        }
        #endregion

        #region Delete Client Contact
        public async Task<SuccessResponse> DeleteClientContact(long clientContactId)
        {
            SuccessResponse successResponse = new SuccessResponse();
            if (clientContactId <= 0)
                successResponse.IsDeleted = false;
            var clientContact = await _iCRMUow.CRMClientContactRepo.GetCRMClientContactByClientContactId(clientContactId, true) ?? throw new BusinessException("Id not found");

            if (clientContact != null && clientContact.Id > 0)
            {
                await _iCRMUow.BeginTransactionAsync();
                _iCRMUow.CRMClientContactRepo.Delete(clientContact);
                await _iCRMUow.SaveChangesAsync();
                successResponse.IsDeleted = true;
                successResponse.Msg.InfoMessage = _mapper.Map<uMessageDto>(await _iCRMUow.MessageRepo.GetMessageByNo(2026));
                successResponse.Msg.InfoMessage.Msg = string.Format(successResponse.Msg.InfoMessage.Msg, "Client Contact");
                await _iCRMUow.CommitTransactionAsync();
                return successResponse;
            }
            else
            {
                successResponse.IsDeleted = false;
                successResponse.Msg.ErrorMessage ??= new List<uMessageDto>();
                var message = (await _iCRMUow.MessageRepo.GetMessageByNo(2038));
                throw new BusinessException(message.Msg, HttpStatusCode.BadRequest);
            }
        }
        #endregion

        #endregion

        #region Client Documents

        #region Create Client Documents
        public async Task<CRMClientDocumentDto> CreateClientDocumentAsync()
        {
            return new CRMClientDocumentDto();
        }
        #endregion

        #region Save Client Documents
        public virtual async Task<CRMClientDocumentDto> SaveClientDocumentAsync(CRMClientDocumentDto request)
        {
            var savedFile = await _iDocumentService.SaveDocumentFile(request.DocumentFile);
            request.DocumentId = savedFile.Id;
            request.DocumentFile = null;

            var clientDocument = _mapper.Map<CRMClientDocument>(request);

            var savedEntity = clientDocument.Id > 0
                ? await _iCRMUow.CRMClientDocumentRepo.UpdateCRMClientDocumentAsync(clientDocument)
                : await _iCRMUow.CRMClientDocumentRepo.InsertCRMClientDocumentAsync(clientDocument);

            await _iCRMUow.SaveChangesAsync();

            var response = _mapper.Map<CRMClientDocumentDto>(savedEntity);
            response.DocumentFile = savedFile;
            var message = await _iCRMUow.MessageRepo.GetMessageByNo(7001);
            response.Msg.InfoMessage = _mapper.Map<uMessageDto>(message);

            return response;
        }
        #endregion

        #region Get All Client Documents By ClientId
        public async Task<CRMClientDocumentSectionDto> GetClientDocumentsByClientIdAsync(long clientId)
        {
            if (clientId <= 0) throw new BusinessException("ID Is Invalid");

            return await _iCRMUow.CRMClientDocumentRepo.GetAllDocumentsByClientIdAsync(clientId);
        }
        #endregion

        #region Delete Client Document
        public virtual async Task<SuccessResponse> DeleteClientDocumentAsync(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            if (id <= 0)
                successResponse.IsDeleted = false;

            var clientDocument = _iCRMUow.CRMClientDocumentRepo
                                    .GetByIdAsync(id).Result
                                    ?? throw new BusinessException("Id not found");

            if (clientDocument != null && clientDocument.Id > 0)
            {
                long documentId = clientDocument.DocumentId ?? 0;

                await _iCRMUow.BeginTransactionAsync();

                _iCRMUow.CRMClientDocumentRepo.Delete(clientDocument);
                await _iCRMUow.SaveChangesAsync();

                if (documentId > 0)
                {
                    try
                    {
                        await _iDocumentService.DeleteDocumentFile(documentId);
                    }
                    catch
                    {
                    }
                }

                await _iCRMUow.CommitTransactionAsync();

                successResponse.IsDeleted = true;
                successResponse.Msg.InfoMessage = _mapper.Map<uMessageDto>(
                    await _iCRMUow.MessageRepo.GetMessageByNo(2026));
                successResponse.Msg.InfoMessage.Msg = string.Format(
                    successResponse.Msg.InfoMessage.Msg, "Client Document");

                return successResponse;
            }
            else
            {
                successResponse.IsDeleted = false;
                successResponse.Msg.ErrorMessage ??= new List<uMessageDto>();
                var message = await _iCRMUow.MessageRepo.GetMessageByNo(2038);
                throw new BusinessException(message.Msg, HttpStatusCode.BadRequest);
            }
        }
        #endregion

        #endregion

        #region Client Service

        #region CRMClient Service Search
        public virtual async Task<CRMClientServiceSearchDto> GetClientServiceSearchAsync()
        {
            return new CRMClientServiceSearchDto();
        }

        public virtual async Task<SearchResult<CRMClientServiceSearchResultsDto>> SearchCRMClientServiceAsync(CRMClientServiceSearchDto request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            if (string.IsNullOrEmpty(request.OrderByColumnName))
                request.OrderByColumnName = nameof(CRMClientServiceSearchResultsDto.ClientServiceId);

            var result = await _iCRMUow.CRMClientServiceSearchRepo.SearchAsync(request);
            if (result != null)
            {
                result.Msg ??= new AppMessage();
                result.Msg.InfoMessage = result.TotalCount > 0
                    ? _mapper.Map(await _iCRMUow.MessageRepo.GetMessageByNo(4, result.TotalCount), result.Msg.InfoMessage)
                    : _mapper.Map(await _iCRMUow.MessageRepo.GetMessageByNo(3), result.Msg.InfoMessage);
                return result;
            }

            return new SearchResult<CRMClientServiceSearchResultsDto>();
        }
        #endregion

        #region initial data        
        public async Task<DDLData> GetClientServiceInitialDataAsync()
        {
            return await _iCRMUow.CRMClientServiceRepo.ClientServiceInitialData();
        }
        #endregion

        #region Create Client Service
        public async Task<CRMClientServiceDto> CreateClientServiceAsync()
        {
            return new CRMClientServiceDto
            {
                ServiceStatusConfigId = 23,
                ServiceStatusDescription = "Open"
            };
        }

        #endregion

        #region Save Client Service
        public async Task<CRMClientServiceDto> SaveClientServiceAsync(CRMClientServiceDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(CRMClientServiceDto));
            var ClientService = _mapper.Map<CRMClientService>(request);

            string groupName = CRMConstants.GroupName.CSO;
            var validUser = await _s2SLogic.Admin.IsUserBasedOnConfiguredGroup(_UserProfile.CurrentUser, groupName);
            if (validUser != true) throw new BusinessException(await _iCRMUow.MessageRepo.GetMessageByNo(1001), HttpStatusCode.BadRequest);

            ClientService.ValidateMandatoryFieldsForService();

            if (ClientService.HasError)
                throw new BusinessException(ClientService.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);

            var legalOfficerAppointment = _mapper.Map<LegalOfficerAppoinment>(request.LegalOfficerAppoinment);
            if (request.Id > 0)
            {
                legalOfficerAppointment.ValidateMandatoryFields();

                if (legalOfficerAppointment.HasError)
                    throw new BusinessException(
                        legalOfficerAppointment.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);


                ClientService.LegalOfficerAppoinment = legalOfficerAppointment;

                await GenerateAppointmentNo(ClientService.LegalOfficerAppoinment);
            }
            else
            {
                ClientService.LegalOfficerAppoinment = null;
            }

            if (string.IsNullOrWhiteSpace(ClientService.ServiceRefNo))
            {
                await GenerateClientServiceNo(ClientService);
            }
            request.LegalOfficerAppoinment.AppoinmentNo = ClientService.LegalOfficerAppoinment.AppoinmentNo;
            request.LegalOfficerAppoinment.ClientServiceId = ClientService.Id;
            ClientService = ClientService.Id > 0 ? await _iCRMUow.CRMClientServiceRepo.UpdateClientService(request) : await _iCRMUow.CRMClientServiceRepo.InsertClientService(ClientService);

            await _iCRMUow.SaveChangesAsync();
            if (ClientService.Id > 0 && ClientService.LegalOfficerAppoinment != null)
            {
                ClientService.LegalOfficerAppoinment.ClientServiceId = ClientService.Id;
            }

            await _iCRMUow.SaveChangesAsync();

            var response = _mapper.Map<CRMClientServiceDto>(ClientService);

            await SetDescription(response);
            await SetDescription(response.LegalOfficerAppoinment);

            return response;
        }

        private async Task<CRMClientService> GenerateClientServiceNo(CRMClientService ClientService)
        {
            await _iCRMUow.CRMClientServiceRepo.GenerateClientServiceRefNo(ClientService);
            if (string.IsNullOrWhiteSpace(ClientService.ServiceRefNo))
            {
                throw new BusinessException(await _iCRMUow.MessageRepo.GetMessageByNo(CRMConstants.CRMClientReferenceNumber.ErrorGeneratingRefNo, "Client Ref Number"), HttpStatusCode.BadRequest);
            }

            return ClientService;
        }
        #endregion

        #region Get Client Service By Id
        public async Task<CRMClientServiceDto> GetClientServiceByClientServiceIdAsync(long clientServicetId)
        {
            var clientService = _mapper.Map<CRMClientServiceDto>(await _iCRMUow.CRMClientServiceRepo.GetClientServiceById(clientServicetId));
            await _iCRMUow.ConfigRepo.SetDescription(clientService);
            await _iCRMUow.ConfigRepo.SetDescription(clientService.LegalOfficerAppoinment);

            return clientService;
        }
        #endregion

        #region Get Client Service Status History By Id
        public async Task<List<CRMClientServiceStatusHistoryDto>> GetClientServiceStatusHistoryByClientServiceIdAsync(long clientServicetId)
        {
            var list = await _iCRMUow.CRMClientServiceStatusHistoryRepo.GetClientServiceStatusHistoryById(clientServicetId);
            foreach (var item in list)
            {
                await _iCRMUow.ConfigRepo.SetDescription(item);
            }
            return list;
        }
        #endregion

        #region Get Client Service Email History By Id
        public async Task<List<CRMClientServiceEmailHistoryDto>> GetClientServiceEmailHistoryByClientServiceIdAsync(long clientServicetId)
        {
            return await _iCRMUow.CRMClientServiceEmailHistoryRepo.GetClientServiceEmailHistoryById(clientServicetId);
        }
        #endregion

        #region Get Client Service Assigned Officer History By Id
        public async Task<List<CRMClientServiceAssignedOfficerHistoryDto>> GetClientServiceAssignedOfficerHistoryByClientServiceIdAsync(long clientServicetId)
        {
            var list = await _iCRMUow.CRMClientServiceAssignedOfficerHistoryRepo.GetClientServiceAssignedOfficerHistoryById(clientServicetId);
            foreach (var item in list)
            {
                await _iCRMUow.ConfigRepo.SetDescription(item);
            }
            return list;
        }
        #endregion

        #region Get Client Service Notes By Id
        public async Task<List<CRMClientServiceNotesDto>> GetClientServiceNotesByClientServiceIdAsync(long clientServicetId)
        {
            var list = await _iCRMUow.CRMClientServiceNotesRepo.GetClientServiceNotesById(clientServicetId);
            foreach (var item in list)
            {
                await _iCRMUow.ConfigRepo.SetDescription(item);
            }
            return list;
        }
        #endregion

        #region Get All Client Services By ClientId
        public async Task<CRMClientServiceSectionDto> GetAllClientServiceByClientId(long clientId)
        {
            if (clientId <= 0) throw new BusinessException("ID Is Invalid");
            var result = await _iCRMUow.CRMClientServiceRepo.GetAllClientServiceByClientIdAsync(clientId);

            foreach (var item in result.Items)
            {
                await _iCRMUow.ConfigRepo.SetDescription(item);
            }

            return result;
        }
        #endregion

        #endregion

        #region Legal Officer

        #region Legal Officer Search
        public virtual async Task<LegalOfficerSearchDto> GetLegalOfficerSearchAsync()
        {
            return new LegalOfficerSearchDto();
        }

        public virtual async Task<SearchResult<LegalOfficerSearchResultsDto>> SearchLegalOfficerAsync(LegalOfficerSearchDto request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            if (string.IsNullOrEmpty(request.OrderByColumnName))
                request.OrderByColumnName = nameof(LegalOfficerSearchResultsDto.LegalOfficerId);
            var result = await _iCRMUow.LegalOfficerSearchRepo.SearchAsync(request);
            if (result != null)
            {
                result.Msg ??= new AppMessage();
                result.Msg.InfoMessage = result.TotalCount > 0
                    ? _mapper.Map(await _iCRMUow.MessageRepo.GetMessageByNo(4, result.TotalCount), result.Msg.InfoMessage)
                    : _mapper.Map(await _iCRMUow.MessageRepo.GetMessageByNo(3), result.Msg.InfoMessage);
                return result;
            }

            return new SearchResult<LegalOfficerSearchResultsDto>();
        }
        #endregion

        #region initial data        
        public async Task<DDLData> GetLegalOfficerInitialDataAsync()
        {
            return await _iCRMUow.LegalOfficerRepo.GetLegalOfficerInitialData();
        }
        #endregion

        #region Create Legal Officer
        public async Task<LegalOfficerDto> CreateLegalOfficerAsync()
        {
            return new LegalOfficerDto();
        }
        #endregion

        #region Save Legal Officer
        public async Task<LegalOfficerDto> SaveLegalOfficerAsync(LegalOfficerDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var legalOfficer = _mapper.Map<LegalOfficer>(request);
            string groupName = CRMConstants.GroupName.LAO;
            var validUser = await _s2SLogic.Admin
                                 .IsUserBasedOnConfiguredGroup(_UserProfile.CurrentUser, groupName);
            if (validUser != true)
                throw new BusinessException("Logged User is not allowed to perform this task");

            legalOfficer.ValidateMandatoryFieldsForLegalOfficer();
            var residentialAddress = _mapper.Map<Address>(request.ResidentialAddress);
            residentialAddress.ValidateMandatoryFields();

            if (legalOfficer.HasError)
                throw new BusinessException(
                    legalOfficer.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);
            if (residentialAddress.HasError)
                throw new BusinessException(
                    residentialAddress.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);

            var photo = await _iDocumentService.SaveDocumentFile(request.Photo);

            if (photo.Id > 0)
                legalOfficer.Photo = null;
            var doc = await _iDocumentService.SaveDocumentFile(request.Doc);
            if (doc.Id > 0)
                legalOfficer.Doc = null;

            legalOfficer.ResidentialAddress = residentialAddress;
            legalOfficer.PhotoId = photo.Id;
            legalOfficer.IDDocId = doc.Id;


            if (legalOfficer.Id > 0)
                await _iCRMUow.LegalOfficerRepo.UpdateLegalOfficer(request);
            else
                await _iCRMUow.LegalOfficerRepo.InsertLegalOfficer(legalOfficer);

            await _iCRMUow.SaveChangesAsync();

            var response = _mapper.Map<LegalOfficerDto>(legalOfficer);
            response.Photo = photo;
            response.Doc = doc;
            await SetDescription(response);
            await _iCRMUow.ConfigRepo.SetAddressDescription(response.ResidentialAddress);
            var userDetails = await _iCRMUow.LegalOfficerRepo.GetDetailsFromITGUser(legalOfficer.UserSerialId);

            if (userDetails != null)
            {
                response.EmailId = userDetails.Constant;
                response.ContactNo = userDetails.Description;
            }

            return response;
        }
        #endregion

        #region Get Legal Officer By Id
        public async Task<LegalOfficerDto> GetLegalOfficerByLegalOfficerIdAsync(long LegalOfficerId)
        {
            var legalOfficer = await _iCRMUow.LegalOfficerRepo.GetLegalOfficerById(LegalOfficerId);
            var userDetails = await _iCRMUow.LegalOfficerRepo.GetDetailsFromITGUser(legalOfficer.UserSerialId);

            if (userDetails != null)
            {
                legalOfficer.ContactNo = userDetails.Constant;
                legalOfficer.EmailId = userDetails.Description;
                legalOfficer.Name = userDetails.FilterKey;
            }
            if (legalOfficer.Photo != null)
                legalOfficer.Photo.base64FileContent = await GetBase64FromFile(
                    legalOfficer.Photo.RelativePath,
                    legalOfficer.Photo.FileName
                );
            if (legalOfficer.Doc != null)
                legalOfficer.Doc.base64FileContent = await GetBase64FromFile(
                    legalOfficer.Doc.RelativePath,
                    legalOfficer.Doc.FileName
                );

            await SetDescription(legalOfficer);
            await _iCRMUow.ConfigRepo.SetAddressDescription(legalOfficer.ResidentialAddress);

            return _mapper.Map<LegalOfficerDto>(legalOfficer);
        }
        #endregion

        #region Delete LegalOfficer
        public async Task<SuccessResponse> DeleteLegalOfficer(long LegalOfficerId)
        {
            SuccessResponse successResponse = new SuccessResponse();
            if (LegalOfficerId <= 0)
                successResponse.IsDeleted = false;
            var LegalOfficer = await _iCRMUow.LegalOfficerRepo.GetLegalOfficerById(LegalOfficerId, true) ?? throw new BusinessException("Id not found");

            if (LegalOfficer != null && LegalOfficer.Id > 0)
            {
                await _iCRMUow.BeginTransactionAsync();
                _iCRMUow.LegalOfficerRepo.Delete(_mapper.Map<LegalOfficer>(LegalOfficer));
                await _iCRMUow.SaveChangesAsync();
                successResponse.IsDeleted = true;
                successResponse.Msg.InfoMessage = _mapper.Map<uMessageDto>(await _iCRMUow.MessageRepo.GetMessageByNo(2026));
                successResponse.Msg.InfoMessage.Msg = string.Format(successResponse.Msg.InfoMessage.Msg, "Legal Officer");
                await _iCRMUow.CommitTransactionAsync();
                return successResponse;
            }
            else
            {
                successResponse.IsDeleted = false;
                successResponse.Msg.ErrorMessage ??= new List<uMessageDto>();
                var message = (await _iCRMUow.MessageRepo.GetMessageByNo(2038));
                throw new BusinessException(message.Msg, HttpStatusCode.BadRequest);
            }
        }
        #endregion

        #region  GetLegalOfficerIdbyUserLoginId
        public async Task<long> GetLegalOfficerIdbyUserLoginId(string userLoginId)
        {
            return await _iCRMUow.LegalOfficerScheduleRepo.GetLegalOfficerIdbyUserLoginId(userLoginId);
        }

        #endregion


        #region Load LegalOfficer Schedule     
        public async Task<List<LegalOfficerSchedulesDto>> LoadLegalOfficerSchedule(long LegalOfficerId)
        {
            return await _iCRMUow.LegalOfficerScheduleRepo.LoadLegalOfficerSchedule(LegalOfficerId);
        }
        #endregion

        #region save Legal Officer Schedule
        public async Task<List<LegalOfficerSchedulesDto>> SaveLegalOfficerSchedules(LegalOfficerSchedulesDto request)
        {
            List<LegalOfficerSchedulesDto> list = new List<LegalOfficerSchedulesDto>();
            var LegalOfficerSchedule = _mapper.Map<LegalOfficerSchedules>(request);
            LegalOfficerSchedule.ValidateMandatoryFieldsForLegalOfficerSchedule();

            if (LegalOfficerSchedule.HasError)
                throw new BusinessException(LegalOfficerSchedule.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);

            LegalOfficerSchedule = LegalOfficerSchedule.Id > 0 ? await _iCRMUow.LegalOfficerScheduleRepo.UpdateLegalOfficerSchedule(request)
                : await _iCRMUow.LegalOfficerScheduleRepo.InsertLegalOfficerSchedule(request);

            await _iCRMUow.SaveChangesAsync();
            var response = _mapper.Map<LegalOfficerSchedulesDto>(LegalOfficerSchedule);
            list = await _iCRMUow.LegalOfficerScheduleRepo.LoadLegalOfficerSchedule(request.LegalOfficerId);
            return list;
        }
        #endregion

        #region Legal Officer Appointment

        #region Legal Officer Appointment initial data        
        public async Task<DDLData> GetLegalOfficerAppoinmentInitialDataAsync()
        {
            return await _iCRMUow.LegalOfficerAppoinmentRepo.GetLegalOfficerAppoinmentInitialData();
        }
        #endregion

        #region Legal Officer Appoinment Search
        public virtual async Task<LegalOfficerAppoinmentSearchDto> GetLegalOfficerAppoinmentSearchAsync()
        {
            return new LegalOfficerAppoinmentSearchDto();
        }

        public virtual async Task<SearchResult<LegalOfficerAppoinmentSearchResultsDto>> SearchLegalOfficerAppoinmentAsync(LegalOfficerAppoinmentSearchDto request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            if (string.IsNullOrEmpty(request.OrderByColumnName))
                request.OrderByColumnName = nameof(LegalOfficerAppoinmentSearchResultsDto.LegalOfficerAppoinmentId);
            var result = await _iCRMUow.LegalOfficerAppoinmentSearchRepo.SearchAsync(request);
            if (result != null)
            {
                result.Msg ??= new AppMessage();
                result.Msg.InfoMessage = result.TotalCount > 0
                    ? _mapper.Map(await _iCRMUow.MessageRepo.GetMessageByNo(4, result.TotalCount), result.Msg.InfoMessage)
                    : _mapper.Map(await _iCRMUow.MessageRepo.GetMessageByNo(3), result.Msg.InfoMessage);
                return result;
            }

            return new SearchResult<LegalOfficerAppoinmentSearchResultsDto>();
        }
        #endregion

        #region Create Legal Officer Appointment
        public Task<LegalOfficerAppoinmentDto> CreateLegalOfficerAppoinmentAsync()
        {
            return Task.FromResult(new LegalOfficerAppoinmentDto());
        }
        #endregion

        #region Save Legal Officer Appointment
        public async Task<LegalOfficerAppoinmentDto> SaveLegalOfficerAppoinmentAsync(LegalOfficerAppoinmentDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(LegalOfficerAppoinmentDto));

            var appoinment = _mapper.Map<LegalOfficerAppoinment>(request);

            appoinment.AppoinmentDate = DateTime.Parse(request.AppoinmentDate);
            appoinment.StartTime = TimeSpan.Parse(request.StartTime);
            appoinment.EndTime = TimeSpan.Parse(request.EndTime);

            bool isOverlap = await _iCRMUow.LegalOfficerAppoinmentRepo.IsSlotAlreadyBookedAsync(
                appoinment.LegalOfficerId,
                appoinment.AppoinmentDate,
                appoinment.StartTime,
                appoinment.EndTime
            );

            if (isOverlap) throw new BusinessException(
                    "This time slot is already booked. Please choose another.", HttpStatusCode.Conflict);

            if (appoinment.Id > 0)
            {
                await _iCRMUow.LegalOfficerAppoinmentRepo.UpdateLegalOfficerAppoinmentAsync(request);
            }
            else
            {
                await GenerateAppointmentNo(appoinment);
                await _iCRMUow.LegalOfficerAppoinmentRepo.InsertLegalOfficerAppoinmentAsync(appoinment);
            }

            await _iCRMUow.SaveChangesAsync();

            var response = _mapper.Map<LegalOfficerAppoinmentDto>(appoinment);

            await SetDescription(response);

            return response;
        }

        private async Task<LegalOfficerAppoinment> GenerateAppointmentNo(LegalOfficerAppoinment appoinment)
        {
            await _iCRMUow.LegalOfficerAppoinmentRepo.GenerateAppointmentRefNo(appoinment);

            if (string.IsNullOrWhiteSpace(appoinment.AppoinmentNo))
                throw new BusinessException(await _iCRMUow.MessageRepo.
                    GetMessageByNo(CRMConstants.AppoinmentReferenceNumber.ErrorGeneratingRefNo,
                        "Appointment Ref Number"),
                    HttpStatusCode.BadRequest
                );

            return appoinment;
        }
        #endregion

        #region Get Legal Officer Appointment By Appoinment Id
        public async Task<LegalOfficerAppoinmentDto> GetLegalOfficerAppoinmentByAppoinmentIdAsync(long LegalOfficerAppoinmentId)
        {
            var appoinment = await _iCRMUow.LegalOfficerAppoinmentRepo.GetLegalOfficerAppoinmentById(LegalOfficerAppoinmentId);

            if (appoinment == null) return null;
            var appoinmentDto = _mapper.Map<LegalOfficerAppoinmentDto>(appoinment);
            await SetDescription(appoinmentDto);

            return appoinmentDto;
        }
        #endregion

        #region Get Appoinment Calendar Async
        public async Task<List<AppoinmentCalendarDto>> GetAppoinmentCalendarAsync(long legalOfficerId, int month, int year)
        {
            List<AppoinmentCalendarDto> calender = new List<AppoinmentCalendarDto>();
            // Step 1: Validate inputs
            if (legalOfficerId <= 0)
                throw new BusinessException("Invalid Legal Officer Id.", HttpStatusCode.BadRequest);

            if (month < 1 || month > 12)
                throw new BusinessException("Invalid Month.", HttpStatusCode.BadRequest);

            if (year < 2000)
                throw new BusinessException("Invalid Year.", HttpStatusCode.BadRequest);

            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            for (var date = startDate; date < endDate; date = date.AddDays(1))
            {
                var appointments = await _iCRMUow.LegalOfficerAppoinmentRepo.GetAppoinmentTimeSlotsByDateAsync(legalOfficerId, date);
                var respones = _mapper.Map<List<AppoinmentTimeSlotsDto>>(appointments);
                var data = await _iCRMUow.LegalOfficerScheduleRepo.LoadLegalOfficerSchedule(legalOfficerId);
                var currentDay = (int)Convert.ToDateTime(date).Date.DayOfWeek;
                var items = data.Where(x => x.DayOffWeek == currentDay).FirstOrDefault();
                AppoinmentCalendarDto cal = new AppoinmentCalendarDto();
                cal.AppoinmentDate = date.ToString();
                cal.BookedCount = appointments.Count;
                var dto = LoadSlotPreview(items);
                if (items.ISActive == "Y")
                {
                    cal.TotalAppointments = dto.Result.SlotPreview.Count;
                    cal.AvailableCount = dto.Result.SlotPreview.Count - appointments.Count;
                    if (appointments.Count == dto.Result.SlotPreview.Count)
                    {

                        cal.DayStatus = "Full";
                    }
                    else if (appointments.Count == 0)
                    {

                        cal.DayStatus = "Available";
                    }
                    else
                    {
                        cal.DayStatus = "Partial";
                    }
                }
                else
                {
                    cal.AvailableCount = 0;
                    cal.DayStatus = "NotAvailable";
                }

                calender.Add(cal);
            }

            var response = _mapper.Map<List<AppoinmentCalendarDto>>(calender);
            return response;
        }
        #endregion

        #region Get Appoinment Time Slots By Date Async
        public async Task<List<AppoinmentTimeSlotsDto>> GetAppoinmentTimeSlotsByDateAsync(long legalOfficerId, string date)
        {
            List<AppoinmentTimeSlotsDto> app = new List<AppoinmentTimeSlotsDto>();
            if (legalOfficerId <= 0) throw new BusinessException("Invalid Legal Officer Id.", HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(date))
                throw new BusinessException("Date is required.", HttpStatusCode.BadRequest);

            if (!DateTime.TryParse(date, out DateTime parsedDate))
                throw new BusinessException("Invalid Date format. Please use yyyy-MM-dd.", HttpStatusCode.BadRequest);

            var appointments = await _iCRMUow.LegalOfficerAppoinmentRepo.GetAppoinmentTimeSlotsByDateAsync(legalOfficerId, parsedDate);
            var respones = _mapper.Map<List<AppoinmentTimeSlotsDto>>(appointments);

            var data = await _iCRMUow.LegalOfficerScheduleRepo.LoadLegalOfficerSchedule(legalOfficerId);
            var currentDay = (int)Convert.ToDateTime(date).Date.DayOfWeek;
            var items = data.Where(x => x.DayOffWeek == currentDay).FirstOrDefault();
            if (items.ISActive == "Y")
            {
                var dto = LoadSlotPreview(items);
                app = dto.Result.SlotPreview
                        .Select(x => {
                            var matchedAppointment = respones.FirstOrDefault(y =>
                                y.StartTime == x.SlotStart &&
                                y.EndTime == x.SlotEnd);

                            return new AppoinmentTimeSlotsDto
                            {
                                StartTime = x.SlotStart,
                                EndTime = x.SlotEnd,

                                IsBooked = matchedAppointment?.IsBooked ?? "N",
                                ClientName = matchedAppointment?.ClientName ?? string.Empty,
                                Notes = matchedAppointment?.Notes ?? string.Empty
                            };
                        })
                        .ToList();
            }

            var response = _mapper.Map<List<AppoinmentTimeSlotsDto>>(app);
            foreach (var slot in response)
            {
                await SetDescription(slot);
            }
            return response;
        }
        #endregion

        #endregion

        #region Legal Officer Schedule

        #region Create Legal Officer Schedule 
        public Task<LegalOfficerSchedulesDto> CreateLegalOfficerScheduleAsync()
        {
            return Task.FromResult(new LegalOfficerSchedulesDto());
        }
        #endregion

        #region Load Slot Preview
        public async Task<LegalOfficerSchedulesDto> LoadSlotPreview(LegalOfficerSchedulesDto request)
        {
            // Step 1 — Validate inputs exist
            // Strings coming from frontend can be null/empty — always guard
            if (string.IsNullOrEmpty(request.StartTime) ||
                string.IsNullOrEmpty(request.EndTime) ||
                request.SlotDuration == null)
                throw new BusinessException(
                    new List<string> { "StartTime, EndTime and SlotDuration are required." },
                    HttpStatusCode.BadRequest);

            // Step 2 — Parse strings to TimeSpan for arithmetic
            // "09:00 AM" → TimeSpan so we can do time math
            // DateTime.Parse handles "09:00 AM" format correctly
            var startTime = DateTime.Parse(request.StartTime).TimeOfDay;
            var endTime = DateTime.Parse(request.EndTime).TimeOfDay;
            var slotDuration = TimeSpan.FromMinutes(request.SlotDuration.Value);

            // Step 3 — Parse optional break times
            bool hasBreak = !string.IsNullOrEmpty(request.BreakStartTime) &&
                            !string.IsNullOrEmpty(request.BreakEndTime);

            TimeSpan? breakStart = hasBreak
                ? DateTime.Parse(request.BreakStartTime!).TimeOfDay
                : null;

            TimeSpan? breakEnd = hasBreak
                ? DateTime.Parse(request.BreakEndTime!).TimeOfDay
                : null;

            // Step 4 — Business rule validations
            if (endTime <= startTime)
                throw new BusinessException(
                    new List<string> { "End time must be after Start time." },
                    HttpStatusCode.BadRequest);

            if (hasBreak && breakEnd <= breakStart)
                throw new BusinessException(
                    new List<string> { "Break end time must be after Break start time." },
                    HttpStatusCode.BadRequest);

            // Step 5 — Sliding window algorithm to generate slots
            var slots = new List<SlotDto>();
            var current = startTime;

            while (current + slotDuration <= endTime)
            {
                var slotEnd = current + slotDuration;

                // If slot overlaps break window → jump to break end
                if (hasBreak &&
                    current < breakEnd &&
                    slotEnd > breakStart)
                {
                    current = breakEnd!.Value; // Resume after break
                    continue;
                }

                slots.Add(new SlotDto
                {
                    SlotStart = current.ToString(),
                    SlotEnd = slotEnd.ToString()
                });

                current = slotEnd; // Slide window forward
            }

            // Step 6 — Build response reusing same request DTO (your pattern)
            request.SlotPreview = slots;
            request.BreakTimeLabel = hasBreak
                ? $"Break Time {breakStart!.Value} - {breakEnd!.Value}"
                : null;

            return await Task.FromResult(request);
        }

        // Private helper — single place for time formatting

        #endregion

        #endregion

        #endregion

        #region Legal Officer Blocked Dates

        #region Legal Officer Blocked Date Search
        public virtual async Task<LegalOfficerBlockedDateSearchDto> GetLegalOfficerBlockedDatesSearchAsync()
        {
            return new LegalOfficerBlockedDateSearchDto();
        }

        public virtual async Task<SearchResult<LegalOfficerBlockedDateSearchResultsDto>> SearchLegalOfficerBlockedDatesAsync(LegalOfficerBlockedDateSearchDto request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            if (string.IsNullOrEmpty(request.OrderByColumnName))
                request.OrderByColumnName = nameof(LegalOfficerBlockedDateSearchResultsDto.LegalOfficerBlockedDateId);
            var result = await _iCRMUow.LegalOfficerBlockedDateSearchRepo.SearchAsync(request);
            if (result != null)
            {
                result.Msg ??= new AppMessage();
                result.Msg.InfoMessage = result.TotalCount > 0
                    ? _mapper.Map(await _iCRMUow.MessageRepo.GetMessageByNo(4, result.TotalCount), result.Msg.InfoMessage)
                    : _mapper.Map(await _iCRMUow.MessageRepo.GetMessageByNo(3), result.Msg.InfoMessage);
                return result;
            }

            return new SearchResult<LegalOfficerBlockedDateSearchResultsDto>();
        }
        #endregion

        #region Create Legal Officer Block date
        public async Task<LegalOfficerBlockedDatesDto> CreateLegalOfficerBlockDate()
        {
            return new LegalOfficerBlockedDatesDto();
        }
        #endregion

        #region SaveLegalOfficerBlockDate
        public async Task<List<LegalOfficerBlockedDatesDto>> SaveLegalOfficerBlockDate(LegalOfficerBlockedDatesDto request)
        {
            List<LegalOfficerBlockedDatesDto> list = new List<LegalOfficerBlockedDatesDto>();
            var LegalOfficerBlockedDates = _mapper.Map<LegalOfficerBlockedDates>(request);
            LegalOfficerBlockedDates.ValidateMandatoryFieldsForLegalOfficerBlockDate();

            if (LegalOfficerBlockedDates.HasError)
                throw new BusinessException(LegalOfficerBlockedDates.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);

            LegalOfficerBlockedDates = LegalOfficerBlockedDates.Id > 0 ? await _iCRMUow.LegalOfficerBlockDateRepo.UpdateLegalOfficerBlockedDates(request)
                : await _iCRMUow.LegalOfficerBlockDateRepo.InsertLegalOfficerBlockedDates(request);

            await _iCRMUow.SaveChangesAsync();
            var response = _mapper.Map<LegalOfficerBlockedDatesDto>(LegalOfficerBlockedDates);
            list = await _iCRMUow.LegalOfficerBlockDateRepo.LoadLegalOfficerBlockDate(request.LegalOfficerId);

            return list;
        }
        #endregion

        #region Get Legal Officer Blocked Date By Id
        public async Task<LegalOfficerBlockedDatesDto> GetLegalOfficerBlockedDateByLegalOfficerBlockDateIdAsync(long legalOfficerBlockDateId)
        {
            var BlockedDate = await _iCRMUow.LegalOfficerBlockDateRepo.GetLegalOfficerBlockedDateByBlockDateId(legalOfficerBlockDateId);

            var legalOfficerBlockedDate = _mapper.Map<LegalOfficerBlockedDatesDto>(BlockedDate);
            await SetDescription(legalOfficerBlockedDate);

            return legalOfficerBlockedDate;
        }
        #endregion

        #region Get Legal Officer Blocked Date By Id
        public async Task<List<LegalOfficerBlockedCalenderDto>> GetLegalOfficerBlockedDateCalenderAsync(long legalOfficerId)
        {
            var BlockedDates = await _iCRMUow.LegalOfficerBlockDateRepo.GetLegalOfficerBlockedDateCalender(legalOfficerId);
            return BlockedDates;
        }
        #endregion

        #endregion


        #region Get Legal Officer Monthly Calendar

        public async Task<LegalOfficerMonthlyCalendarDto> GetLegalOfficerMonthlyCalendarAsync(long legalOfficerId, int year, int month)
        {
            var appointmentEntities = await _iCRMUow.LegalOfficerRepo.GetAppointmentsAsync(legalOfficerId, year, month);

            var blockedDateEntities = await _iCRMUow.LegalOfficerRepo.GetBlockedDatesAsync(legalOfficerId, year, month);

            var appointmentDtos = _mapper.Map<List<CalendarAppointmentDto>>(appointmentEntities);
            var blockedDateDtos = _mapper.Map<List<CalendarBlockedDateDto>>(blockedDateEntities);

            var appointmentsByDay = appointmentEntities
                .GroupBy(a => a.AppoinmentDate.Date)
                .ToDictionary(
                    g => DateOnly.FromDateTime(g.Key),
                    g => _mapper.Map<List<CalendarAppointmentDto>>(g.ToList()));

            var blockedByDay = blockedDateEntities
                .GroupBy(b => b.BlockDate)
                .ToDictionary(
                    g => g.Key,
                    g => _mapper.Map<List<CalendarBlockedDateDto>>(g.ToList()));

            // Build full month calendar
            int totalDays = DateTime.DaysInMonth(year, month);

            var calendarDays = Enumerable.Range(1, totalDays)
                .Select(day => {
                    var date = new DateOnly(year, month, day);

                    return new CalendarDayDto
                    {
                        Date = date,
                        DayName = date.ToString("dddd"),
                        IsWeekend = date.DayOfWeek == DayOfWeek.Saturday ||
                                       date.DayOfWeek == DayOfWeek.Sunday,
                        Appointments = appointmentsByDay
                                        .GetValueOrDefault(date,
                                            new List<CalendarAppointmentDto>()),
                        BlockedDates = blockedByDay
                                        .GetValueOrDefault(date,
                                            new List<CalendarBlockedDateDto>())
                    };
                }).ToList();

            bool hasAnyData = appointmentEntities.Any() || blockedDateEntities.Any();

            return new LegalOfficerMonthlyCalendarDto
            {
                LegalOfficerId = legalOfficerId,
                Year = year,
                Month = month,
                MonthName = new DateTime(year, month, 1).ToString("MMMM"),
                TotalDays = totalDays,
                Calendar = calendarDays,
                Message = new AppMessage
                {
                    InfoMessage = new uMessageDto
                    {
                        Msg = hasAnyData ? null : "No data found"
                    }
                }
            };
        }
        #endregion
    }
}
