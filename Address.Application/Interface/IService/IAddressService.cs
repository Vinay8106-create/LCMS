using Galaxy.Dto;

using LCMS.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRM.Application

{

    public interface IAddressService

    {

        Task<DDL> GetCountryDDL();

        Task<DDLData> GetAddressInitialData();

        Task<AddressDto> CreateAddress();

        Task<AddressDto> SaveAddress(AddressDto address);

        Task<AddressDto> GetAddress(long addressId);

        Task<SuccessResponse> DeleteAddress(long addressId);

        Task<AddressDto> UpdateAddress(long destinationAddressId, AddressDto SourceAddress);

        Task<AddressDto> CopyAddress(long sourceAddressId);

        Task<Data> GetFullAddress(long addressId);

    }

}

