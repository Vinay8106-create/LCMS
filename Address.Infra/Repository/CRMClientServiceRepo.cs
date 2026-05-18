using AutoMapper;
using CRM.Application;
using CRM.Domain;
using DocumentFormat.OpenXml.Office2010.Excel;
using Galaxy.Domain.Exceptions;
using Galaxy.Domain.Models;
using Galaxy.Dto;
using Galaxy.Infra;
using Galaxy.Utility;
using LCMS.Constants;
using LCMS.Dto;
using LCMS.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public SemaphoreSlim _referencenoLock = new SemaphoreSlim(1, 1);

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
            var legalOfficer = await getAllLegalOfficer();
            dDLData.data.Add(legalOfficer);


            return dDLData;
        }

        public async Task<DDL> GetDDLAsync<TEntity>(string key) where TEntity : class
        {
            var data = await _dbContext.Set<TEntity>()
                .Select(x => new DDLClass
                {
                    Id = EF.Property<int>(x, "ConfigId"),
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
                await _referencenoLock.WaitAsync();    // only one thread can be created at a time
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
                _referencenoLock.Release();
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
            await _referencenoLock.WaitAsync();
            string clientRefNo = null;
            using var reader = await _dbContext.ExecuteSpAsync(CRMConstants.StoredProcedures.APP_SP_GetClientRefNumber);
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
            var clientService = await dbSet.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (clientService == null)
                throw new BusinessException("Unable to locate the clientService. Please refresh and try again.", HttpStatusCode.BadRequest);

            _mapper.Map(request, clientService);

            return clientService;
        }

        public async Task<CRMClientService> InsertClientService(CRMClientService clientService)
        {
            await AddAsync(clientService);

            return clientService;
        }

        public async Task<CRMClientService> GetClientServiceById(long clientServiceId, bool isTracking = false)
        {
            return await GetByIdAsync(clientServiceId, isTracking) ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);
        }

      
        public async Task<List<CRMClientServiceEmailHistoryDto>> GetClientServiceEmailHistoryById(long clientServiceId, bool isTracking = false)
        {
            var query = _dbContext.CRMClientServiceEmailHistory.AsQueryable();

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            var data = await query
                .Where(x => x.ClientServiceId == clientServiceId)
                .ToListAsync();

            return _mapper.Map<List<CRMClientServiceEmailHistoryDto>>(data);
        }

        public async Task<List<CRMClientServiceAssignedOfficerHistoryDto>> GetClientServiceAssignedOfficerHistoryById(long clientServiceId, bool isTracking = false)
        {
            var query = _dbContext.CRMClientServiceAssignedOfficer.AsQueryable();

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            var data = await query
                .Where(x => x.ClientServiceId == clientServiceId)
                .ToListAsync();

            return _mapper.Map<List<CRMClientServiceAssignedOfficerHistoryDto>>(data);
        }

        public async Task<List<CRMClientServiceNotesDto>> GetClientServiceNotesById(long clientServiceId, bool isTracking = false)
        {
            var query = _dbContext.CRMClientServiceNotes.AsQueryable();

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            var data = await query
                .Where(x => x.ClientServiceId == clientServiceId)
                .ToListAsync();

            return _mapper.Map<List<CRMClientServiceNotesDto>>(data);
        }

        public async Task<CRMClientService> GenerateClientServiceRefNo(CRMClientService clientService)
        {
            await _referencenoLock.WaitAsync();
            string serviceRefNo = null;
            using var reader = await _dbContext.ExecuteSpAsync(CRMConstants.StoredProcedures.APP_SP_GetClientRefNumber);
            var resultSets = await reader.ReadAsync();
            serviceRefNo = resultSets.Select(x => x.CustomerReferenceNumber).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(serviceRefNo))
            {
                clientService.ServiceRefNo = serviceRefNo;
            }

            return clientService;
        }
        public async Task<DDL> getAllLegalOfficer()
        {
            var data = await (
                                from lo in _dbContext.LegalOfficer
                                join u in _dbContext.Set<User>() on lo.UserSerialId equals u.Id
                                select new DDLClass
                                {
                                    Id = lo.Id,
                                    Constant = u.UserLoginId,
                                    Description = (u.FirstName ?? "") + " " +
                                                  (u.MiddleName ?? "") + " " +
                                                  (u.LastName ?? "")
                                })
                                .AsNoTracking()
                                .ToListAsync();

            return new DDL
            {
                Key = "DDLLegalOfficer",
                Value = data
            };
        }


        public async Task<CRMClientServiceSectionDto> GetAllClientServiceByClientIdAsync(long clientId)
        {
            var services = await _dbContext.CRMClientService
                .Where(c => c.ClientId == clientId)
                .Join(
                    _dbContext.config_ServiceStatus,
                    service => service.ServiceStatusConfigId,
                    status => status.Id,
                    (service, status) => new {
                        Service = service,
                        ServiceStatusDescription = status.Description
                    }
                )
                .OrderByDescending(x => x.Service.Id)
                .ToListAsync();

            var mappedServices = services.Select(x => {
                var dto = _mapper.Map<CRMClientServiceDto>(x.Service);
                dto.ServiceStatusDescription = x.ServiceStatusDescription;
                return dto;
            }).ToList();

            return new CRMClientServiceSectionDto
            {
                Items = mappedServices
            };
        }
    }
}
