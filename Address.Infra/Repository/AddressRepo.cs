using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Infra;
using LCMS.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Master.Infra
{
    public class AddressRepo : Repository<Address>, IAddressRepo
    {
        private readonly DbContext _dbContext;
        protected readonly IServiceProvider _serviceProvider;

        public AddressRepo(DbContext dbContext, IServiceProvider serviceProvider) : base(dbContext)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }

        public virtual async Task<Address> InsertAddressAsync(Address address)
        {
            await AddAsync(address);

            return address;
        }

        public virtual async Task<Address> UpdateAddressAsync(AddressDto request)
        {
            var existingAddress = Query(x => x.Id == request.Id, true).FirstOrDefault();
            if (existingAddress == null)
                throw new BusinessException("Existing Address not found.Please Refresh and Try again Later", HttpStatusCode.BadRequest);

            // Using DI injection

            _serviceProvider.GetRequiredService<IMapper>().Map(request, existingAddress);
            bool IsObjectChange = Update(existingAddress);

            return existingAddress;
        }

        public async Task<Address> GetAddressById(long AddressId, bool isTracking = false)
        {
            return await GetByIdAsync(AddressId, isTracking);
        }
    }
}

