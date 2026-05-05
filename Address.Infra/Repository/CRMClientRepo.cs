using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Infra;
using Galaxy.Utility;
using LCMS.Constants;
using LCMS.Dto;
using LCMS.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CRM.Infra
{
    public class CRMClientRepo : Repository<CRMClient>, ICRMClientRepo
    {
        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;
        public CRMClientRepo(LCMSDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public SemaphoreSlim _referencenoLoack = new SemaphoreSlim(1, 1);

        public async Task<string> GenerateCustomerReferenceNo()
        {
            try
            {
                await _referencenoLoack.WaitAsync();    // only one thread can be created at a time
                using var reader = await _dbContext.ExecuteSpAsync(LCMSCommonConstants.ReferenceNumber.MetadataName,
                new {
                    ConfigId = CRMConstants.CRMClientReferenceNumber.Id,
                    ConfigConstant = CRMConstants.CRMClientReferenceNumber.Constant,
                    RefMetaDataName = CRMConstants.CRMClientReferenceNumber.MetadataName,
                    PrefixAliceName = CRMConstants.CRMClientReferenceNumber.Prefix,
                });

                var result = await reader.ReadAsync();
                string? refNo = result.SingleOrDefault()?.RefNo;
                return refNo ?? string.Empty;
            }
            catch (Exception ex)
            {
                ErrorLog.Instance.WriteErrorLog(ex);
                throw;
            }
            finally
            {
                _referencenoLoack.Release();
            }
        }

        public async Task<CRMClient> GetCustomerByCustomerRefNo(string ClientRefNo, CancellationToken cancellationToken = default)
        {
            var Customer = await _dbContext.CRMClient.FirstOrDefaultAsync(x => x.RefNo == ClientRefNo, cancellationToken);
            if (Customer == null)
                throw new BusinessException("Client RefNo not found", HttpStatusCode.NotFound);

            return Customer;
        }

        public async Task<CRMClient> GetCustomerById(long customerId, bool isTracking = false)
        {
            return await GetByIdAsync(customerId, isTracking) ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);
        }

        public async Task<CRMClient> InsertCustomer(CRMClient request)
        {
            request.RefNo = await GenerateCustomerReferenceNo();
            await AddAsync(request);
            return request;
        }

        public async Task<CRMClient> UpdateCustomer(CRMClientDto request)
        {
            var customer = await _dbContext.CRMClient.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (customer == null)
                throw new BusinessException("Id not found", HttpStatusCode.BadRequest);
            _mapper.Map(request, customer);
            return customer;
        }

        public async Task<CRMClient> GetAlreadyExist(string contactNo, string emailId)
        {
            return await _dbContext.CRMClient
            .Where(x => x.ContactNo == contactNo || (!string.IsNullOrEmpty(emailId) && x.EmailId == emailId)).FirstOrDefaultAsync() ?? new CRMClient();
        }

        public async Task<CRMClient> GenerateClientRefNo(CRMClient client)
        {
            await _referencenoLoack.WaitAsync();
            string clientRefNo = null;
            using var reader = await _dbContext.ExecuteSpAsync(CRMConstants.StoredProcedured.APP_SP_GetClientRefNumber);
            var resultSets = await reader.ReadAsync();
            clientRefNo = resultSets.Select(x => x.CustomerReferenceNumber).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(clientRefNo))
            {
                client.RefNo = clientRefNo;
            }

            return client;
        }

        public async Task<CRMClient> UpdateClient(CRMClientDto request)
        {
            var client = await dbSet.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (client == null)
                throw new BusinessException("Unable to locate the client. Please refresh and try again.", HttpStatusCode.BadRequest);

            _mapper.Map(request, client);

            return client;
        }

        public async Task<CRMClient> InsertClient(CRMClient client)
        {
            if (client.ResidentialAddress != null && client.ResidentialAddress.Id > 0)
                _dbContext.Entry(client.ResidentialAddress).State = EntityState.Unchanged;

            if (client.CommunicationAddress != null && client.CommunicationAddress.Id > 0)
                _dbContext.Entry(client.CommunicationAddress).State = EntityState.Unchanged;

            await AddAsync(client);

            return client;
        }

        public async Task<CRMClient> GetClientById(long clientId, bool isTracking = false)
        {
            return await GetByIdAsync(clientId, isTracking) ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);
        }


        Task<CRMClient> ICRMClientRepo.GetAlreadyExist(string contactNo, string emailId)
        {
            throw new NotImplementedException();
        }

        public Task<CRMClient> GetClientByClientRefNo(string ClientRefNo, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
