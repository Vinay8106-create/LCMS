using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Dto;
using LCMS.Dto;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CRM.Application
{

    public class ConfigService : IConfigService
    {

        private readonly ICRMUow _iCRMUow;
        private readonly IMapper _mapper;

        public ConfigService(ICRMUow CRMUow, IMapper mapper)
        {
            _iCRMUow = CRMUow;
            _mapper = mapper;
        }


        public virtual async Task<ConfigDto> CreateConfig()
        {

            return _mapper.Map<ConfigDto>(new config_ClientType());

        }

        public virtual async Task<SuccessResponse> DeleteConfig(long Id)
        {

            SuccessResponse successResponse = new SuccessResponse();

            if (Id <= 0)

                successResponse.IsDeleted = false;

            var getAddressId = await _iCRMUow.AddressRepo.GetAddressById(Id, true);

            if (getAddressId != null && getAddressId.Id > 0)
            {

                await _iCRMUow.BeginTransactionAsync();

                _iCRMUow.AddressRepo.Delete(getAddressId);

                await _iCRMUow.SaveChangesAsync();

                successResponse.IsDeleted = true;

                successResponse.Msg.InfoMessage = _mapper.Map<uMessageDto>(await _iCRMUow.MessageRepo.GetMessageByNo(1011));

                successResponse.Msg.InfoMessage.Msg = string.Format(successResponse.Msg.InfoMessage.Msg, "Deleted Successfully");

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

        public virtual async Task<ConfigDto> GetConfig(long Id)
        {

            return _mapper.Map<ConfigDto>(await _iCRMUow.AddressRepo.GetAddressById(Id) ?? throw new BusinessException("Address not found"));

        }


        public virtual async Task<ConfigDto> SaveConfig(ConfigDto request)
        {

            if (request == null) throw new ArgumentNullException(nameof(request));

            var address = _mapper.Map<config_ClientType>(request);

            address = address.Id > 0 ? await _iCRMUow.ConfigRepo.UpdateConfigAsync(request) :

                await _iCRMUow.ConfigRepo.InsertConfigAsync(address);
            try
            {
                await _iCRMUow.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _mapper.Map<ConfigDto>(address);

        }

        public virtual async Task<ConfigDto> UpdateConfig(long Id, ConfigDto updateConfig)
        {

            var ConfigDto = await GetConfig(Id);

            _mapper.Map(updateConfig, ConfigDto);

            return await SaveConfig(ConfigDto);

        }

    }

}

