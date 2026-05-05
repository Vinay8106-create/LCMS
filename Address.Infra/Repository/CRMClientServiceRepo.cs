using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Dto;
using Galaxy.Infra;
using Galaxy.Utility;
using LCMS.Constants;
using LCMS.Dto;
using LCMS.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CRM.Infra
{
    public class CRMClientServiceRepo : Repository<CRMClientService>, ICRMClientServiceRepo
    {
        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;
        public CRMClientServiceRepo(LCMSDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public SemaphoreSlim _referencenoLoack = new SemaphoreSlim(1, 1);
        public async Task<DDLData> ClientServiceInitialData()
        {
            DDLData dDLData = new DDLData();
            var ServiceType = await GetDDLAsync<config_Service>("DDLServiceType");
            dDLData.data.Add(ServiceType);
            var MatterType = await GetDDLAsync<config_MatterType>("DDLMatterType");
            dDLData.data.Add(MatterType);
            var MatterSubType = await GetDDLAsync<config_MatterSubType>("DDLMatterSubType");
            dDLData.data.Add(MatterSubType);
            var ContactMode = await GetDDLAsync<config_ContactMode>("DDLContactMode");
            dDLData.data.Add(ContactMode);
            var ServiceStatus = await GetDDLAsync<config_ServiceStatus>("DDLServiceStatus");
            dDLData.data.Add(ServiceStatus);           
            return dDLData;
        }

        public async Task<DDL> GetDDLAsync<TEntity>(string key) where TEntity : class
        {
            var data = await _dbContext.Set<TEntity>()
                .Select(x => new DDLClass
                {
                    Id = EF.Property<int>(x, "Id"),
                    Description = EF.Property<string>(x, "Description")
                }).AsNoTracking().ToListAsync();

            return new DDL
            {
                Key = key,
                Value = data
            };
        }
        public async Task<string> GenerateCustomerServiceReferenceNo()
        {
            try
            {
                await _referencenoLoack.WaitAsync();    // only one thread can be created at a time
                using var reader = await _dbContext.ExecuteSpAsync(LCMSCommonConstants.ReferenceNumber.MetadataName,
                new
                {
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

      

        public async Task<CRMClientService> GetCustomerServiceById(long customerServiceId, bool isTracking = false)
        {
            return await GetByIdAsync(customerServiceId, isTracking) ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);
        }

        public async Task<CRMClientService> InsertCustomerService(CRMClientService request)
        {
            request.ServiceRefNo = await GenerateCustomerServiceReferenceNo();
            await AddAsync(request);
            return request;
        }

        public async Task<CRMClientService> UpdateCustomerService(CRMClientServiceDto request)
        {
            var customer = await _dbContext.CRMClientService.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (customer == null)
                throw new BusinessException("Id not found", HttpStatusCode.BadRequest);
            _mapper.Map(request, customer);
            return customer;
        }

        

        public async Task<CRMClient> GenerateClientServiceRefNo(CRMClient client)
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

        public async Task<CRMClientService> UpdateClientService(CRMClientServiceDto request)
        {
            var client = await dbSet.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (client == null)
                throw new BusinessException("Unable to locate the client. Please refresh and try again.", HttpStatusCode.BadRequest);

            _mapper.Map(request, client);

            return client;
        }

        public async Task<CRMClientService> InsertClientService(CRMClientService client)
        {
            await AddAsync(client);

            return client;
        }

        public async Task<CRMClientService> GetClientServiceById(long clientServiceId, bool isTracking = false)
        {
            return await GetByIdAsync(clientServiceId, isTracking) ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);
        }

        public Task<CRMClientService> GenerateClientServiceRefNo(CRMClientService client)
        {
            throw new NotImplementedException();
        }
    }
}
