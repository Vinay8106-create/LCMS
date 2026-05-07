using AutoMapper;
using Azure.Core;
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
            return new CRMClientDto();
        }
        #endregion

        #region Save Client
        public async Task<CRMClientDto> SaveClientAsync(CRMClientDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(CRMClientDto));
            var Client = _mapper.Map<CRMClient>(request);

            string groupName = CRMConstants.GroupName.CSO;

            var validUser = await _s2SLogic.Admin.IsUserBasedOnConfiguredGroup(_UserProfile.CurrentUser, groupName);

            if (validUser != true) throw new BusinessException(await _iCRMUow.MessageRepo.GetMessageByNo(1001), HttpStatusCode.BadRequest);

            Client.ValidateMandatoryFields();

            var Residentialaddress = _mapper.Map<Address>(request.ResidentialAddress);
            Residentialaddress.ValidateMandatoryFields();

            var Communicationaddress = _mapper.Map<Address>(request.CommunicationAddress);
            Communicationaddress.ValidateMandatoryFields();

            if (Client.HasError)
                throw new BusinessException(Client.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);
            if (Residentialaddress.HasError)
                throw new BusinessException(Residentialaddress.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);
            if (Communicationaddress.HasError)
                throw new BusinessException(Communicationaddress.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);

            var savedResidentialAddress = await _iAddressService.SaveAddress(request.ResidentialAddress);
            var savedCommunicationAddress = await _iAddressService.SaveAddress(request.CommunicationAddress);
            var photo = await _iDocumentService.SaveDocumentFile(request.Photo);

            if (string.IsNullOrWhiteSpace(Client.RefNo))
            {
                await GenerateClientNo(Client);
            }

            Client.ResidentialAddressId = savedResidentialAddress.Id;
            Client.CommunicationAddressId = savedCommunicationAddress.Id;

            // Break the object reference so EF doesn't re-insert them
            Client.ResidentialAddress = null;
            Client.CommunicationAddress = null;

            Client = Client.Id > 0 ? await _iCRMUow.CRMClientRepo.UpdateClient(request) : await _iCRMUow.CRMClientRepo.InsertClient(Client);

            await _iCRMUow.SaveChangesAsync();
            var response = _mapper.Map<CRMClientDto>(Client);
            response.ResidentialAddress = _mapper.Map<AddressDto>(savedResidentialAddress);
            response.CommunicationAddress = _mapper.Map<AddressDto>(savedCommunicationAddress);
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
        #endregion

        #region Get Client By Client Id
        public async Task<CRMClientDto> GetClientByClientIdAsync(long clientId)
        {
            return _mapper.Map<CRMClientDto>(await _iCRMUow.CRMClientRepo.GetClientById(clientId));
        }
        #endregion

        public Task<CRMClientDocumentDto> CreateClientDocument()
        {
            throw new NotImplementedException();
        }

        public Task<CRMClientDto> GetClientByClientRefNo(string ClientRefNo)
        {
            throw new NotImplementedException();
        }

        public Task<CRMClientContactDto> GetClientContactByClientContactId(long clientcontactId)
        {
            throw new NotImplementedException();
        }


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
            return _mapper.Map<CRMClientContactDto>(await _iCRMUow.CRMClientContactRepo.GetCRMClientContactByClientContactId(clientContactId));
        }
        #endregion

        #region Get All Client Contacts By ClientId
        public async Task<CRMClientContactSectionDto> GetAllClientContactsByClientId(long clientId)
        {
            if (clientId <= 0) throw new BusinessException("ID Is Invalid");

            return await _iCRMUow.CRMClientContactRepo.GetAllClientContactsByClientIdAsync(clientId);
        }
        #endregion

        #region Get All Client Documents By ClientId
        public async Task<CRMClientDocumentSectionDto> GetClientDocumentsByClientIdAsync(long clientId)
        {
            if (clientId <= 0) throw new BusinessException("ID Is Invalid");

            return await _iCRMUow.CRMClientDocumentRepo.GetAllDocumentsByClientIdAsync(clientId);
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
            return new CRMClientServiceDto();
        }
        #endregion

        #region Save Client Service
        public async Task<CRMClientServiceDto> SaveClientServiceAsync(CRMClientServiceDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(CRMClientDto));
            var ClientService = _mapper.Map<CRMClientService>(request);

            string groupName = CRMConstants.GroupName.CSO;
            var validUser = await _s2SLogic.Admin.IsUserBasedOnConfiguredGroup(_UserProfile.CurrentUser, groupName);
            if (validUser != true) throw new BusinessException(await _iCRMUow.MessageRepo.GetMessageByNo(1001), HttpStatusCode.BadRequest);

            ClientService.ValidateMandatoryFieldsForService();

            if (ClientService.HasError)
                throw new BusinessException(ClientService.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(ClientService.ServiceRefNo))
            {
                await GenerateClientServiceNo(ClientService);
            }

            ClientService = ClientService.Id > 0 ? await _iCRMUow.CRMClientServiceRepo.UpdateClientService(request) : await _iCRMUow.CRMClientServiceRepo.InsertClientService(ClientService);

            await _iCRMUow.SaveChangesAsync();
            var response = _mapper.Map<CRMClientServiceDto>(ClientService);

            await SetDescription(response);

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
            var LegalOfficer = _mapper.Map<LegalOfficer>(request);

            string groupName = CRMConstants.GroupName.LAO;
            var validUser = await _s2SLogic.Admin.IsUserBasedOnConfiguredGroup(_UserProfile.CurrentUser, groupName);
            if (validUser != true) throw new BusinessException(await _iCRMUow.MessageRepo.GetMessageByNo(1001), HttpStatusCode.BadRequest);

            LegalOfficer.ValidateMandatoryFieldsForLegalOfficer();

            var Residentialaddress = _mapper.Map<Address>(request.ResidentialAddress);
            Residentialaddress.ValidateMandatoryFields();

            if (LegalOfficer.HasError || Residentialaddress.HasError)
                throw new BusinessException(LegalOfficer.errorMsgList.Select(x => x.Msg).ToList(), HttpStatusCode.BadRequest);

            var savedResidentialAddress = await _iAddressService.SaveAddress(request.ResidentialAddress);

            LegalOfficer.ResidentialAddressId = savedResidentialAddress.Id;
            LegalOfficer.ResidentialAddress = null;
            request.ResidentialAddress = null;

            LegalOfficer = LegalOfficer.Id > 0 ? await _iCRMUow.LegalOfficerRepo.UpdateLegalOfficer(request)
                : await _iCRMUow.LegalOfficerRepo.InsertLegalOfficer(request);

            await _iCRMUow.SaveChangesAsync();
            var response = _mapper.Map<LegalOfficerDto>(LegalOfficer);
            response.ResidentialAddress = _mapper.Map<AddressDto>(savedResidentialAddress);


            var userDetails = await _iCRMUow.LegalOfficerRepo.GetDetailsFromITGUser(response.UserSerialId);

            if (userDetails != null)
            {
                response.EmailId = userDetails.Constant;
                response.ContactNo = userDetails.Description;
            }

            await SetDescription(response);

            return response;
        }
        #endregion

        #region Get Legal Officer By Id
        public async Task<LegalOfficerDto> GetLegalOfficerByLegalOfficerIdAsync(long LegalOfficerId)
        {
            var legalOfficer = await _iCRMUow.LegalOfficerRepo.GetLegalOfficerById(LegalOfficerId);

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
        #endregion
    }
}
