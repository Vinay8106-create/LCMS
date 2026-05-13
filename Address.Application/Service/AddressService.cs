using AutoMapper;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Dto;
using LCMS.Dto;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CRM.Application
{
    public class AddressService : IAddressService
    {
        private readonly ICRMUow _iCRMUow;
        private readonly IMapper _mapper;

        public AddressService(ICRMUow CRMUow, IMapper mapper)
        {
            _iCRMUow = CRMUow;
            _mapper = mapper;
        }

        public virtual async Task<AddressDto> CopyAddress(long addressId)
        {
            var addressDto = await GetAddress(addressId);
            addressDto.Id = 0;
            addressDto.Version = 0;
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                IgnoreReadOnlyProperties = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            options.Converters.Add(new JsonStringEnumConverter());

            return await SaveAddress(addressDto);
        }

        public virtual async Task<AddressDto> CreateAddress()
        {
            return _mapper.Map<AddressDto>(new Address());
        }

        public virtual async Task<SuccessResponse> DeleteAddress(long addressId)
        {
            SuccessResponse successResponse = new SuccessResponse();
            if (addressId <= 0) successResponse.IsDeleted = false;

            var getAddressId = await _iCRMUow.AddressRepo.GetAddressById(addressId, true);

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

        public virtual async Task<AddressDto> GetAddress(long addressId)
        {
            return _mapper.Map<AddressDto>(await _iCRMUow.AddressRepo.GetAddressById(addressId) ?? throw new BusinessException("Address not found"));
        }

        public Task<DDLData> GetAddressInitialData()
        {
            throw new NotImplementedException();
        }

        public Task<DDL> GetCountryDDL()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<Data> GetFullAddress(long addressId)
        {
            var address = await GetAddress(addressId);

            var parts = new[]
            {
                address.Line1,
                address.Line2,
                address.Line3,
                address.Level1Config,
                address.Level2Config,
                address.Level3Config
            };

            var fullAddress = string.Join(" ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

            return new Data { Value = fullAddress };
        }

        public virtual async Task<AddressDto> SaveAddress(AddressDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var address = _mapper.Map<Address>(request);
            address = address.Id > 0 ? await _iCRMUow.AddressRepo.UpdateAddressAsync(request) :
                                                            await _iCRMUow.AddressRepo.InsertAddressAsync(address);


            return _mapper.Map<AddressDto>(address);
        }

        public virtual async Task<AddressDto> UpdateAddress(long addressId, AddressDto updateAddress)
        {
            var addressDto = await GetAddress(addressId);
            _mapper.Map(updateAddress, addressDto);

            return await SaveAddress(addressDto);
        }
    }
}

