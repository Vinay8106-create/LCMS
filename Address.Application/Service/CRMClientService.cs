using AutoMapper;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Domain.Models;
using Galaxy.Dto;
using LCMS.Constants;
using LCMS.Dto;
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
            await _s2SLogic.Admin.IsUserBasedOnConfiguredGroup(_UserProfile.CurrentUser, groupName);
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

        private async Task SetDescription(CRMClientContactDto client)
        {
            await _iCRMUow.ConfigRepo.SetDescription(client);
        }
        #endregion

        public async Task<CRMClientDto> GetClientByClientIdAsync(long clientId)
        {
            return _mapper.Map<CRMClientDto>(await _iCRMUow.CRMClientRepo.GetClientById(clientId));
        }

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

        public Task<CRMClientDto> GetCRMClientByIdAsync(long clientId)
        {
            throw new NotImplementedException();
        }

        public Task<DDLData> GetCRMClientInitialDataAsync()
        {
            throw new NotImplementedException();
        }


        public Task<CRMClientDocumentDto> SaveClientDocumentAsync(CRMClientDocumentDto request)
        {
            throw new NotImplementedException();
        }

        #region Client Details

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
        #region initial data        
        public async Task<DDLData> GetClientServiceInitialDataAsync()
        {
            return await _iCRMUow.CRMClientServiceRepo.ClientServiceInitialData();
        }
        #endregion
        #endregion
    }
}
