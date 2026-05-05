using CRM.Domain;
using Galaxy.Application;
using LCMS.Dto;

namespace CRM.Application
{

    public interface IAddressRepo : IRepository<Address>
    {
        Task<Address> InsertAddressAsync(Address address);

        Task<Address> UpdateAddressAsync(AddressDto request);

        Task<Address> GetAddressById(long AddressId, bool isTracking = false);

    }

}
