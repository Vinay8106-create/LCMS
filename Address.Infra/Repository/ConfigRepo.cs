using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Dto;
using Galaxy.Infra;
using LCMS.Dto;
using LCMS.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace Master.Infra
{
    public class ConfigRepo : Repository<config_ClientType>, IConfigRepo
    {

        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;


        public ConfigRepo(LCMSDbContext dbContext, IMapper mapper, IMemoryCache memoryCache) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public virtual async Task<config_ClientType> InsertConfigAsync(config_ClientType Config)
        {
            await AddAsync(Config);

            return Config;
        }

        public virtual async Task<config_ClientType> UpdateConfigAsync(ConfigDto request)
        {
            var existingAddress = Query(x => x.Id == request.Id, true).FirstOrDefault();
            if (existingAddress == null)
                throw new BusinessException("Existing Address not found.Please Refresh and Try again Later", HttpStatusCode.BadRequest);

            // Using DI injection
            _mapper.Map(request, existingAddress);

            return existingAddress;
        }

        public async Task<config_ClientType> GetConfigById(long Id, bool isTracking = false)
        {
            return await GetByIdAsync(Id, isTracking);
        }

        public async Task<DDLData> ClientInitialData()
        {
            DDLData dDLData = new DDLData();
            var clientType = await GetDDLAsync<config_ClientType>("DDLClientType");
            dDLData.data.Add(clientType);
            var clientSubType = await GetDDLAsync<config_ClientSubType>("DDLClientSubType");
            dDLData.data.Add(clientSubType);
            var config_ClientStatus = await GetDDLAsync<config_ClientStatus>("DDLClientStatus");
            dDLData.data.Add(config_ClientStatus);
            var config_Gender = await GetDDLAsync<config_Gender>("DDLGender");
            dDLData.data.Add(config_Gender);
            var config_MaritalStatus = await GetDDLAsync<config_MaritalStatus>("DDLMaritalStatus");
            dDLData.data.Add(config_MaritalStatus);
            var config_DocumentMaster = await GetDDLAsync<config_DocumentMaster>("DDLDocumentMaster");
            dDLData.data.Add(config_DocumentMaster);
            var config_Addresslevel1 = await GetDDLAsync<config_AddressLevel1>("DDLAddressLevel1");
            dDLData.data.Add(config_Addresslevel1);
            var config_Addresslevel2 = await GetDDLAsync<config_AddressLevel2>("DDLAddressLevel2");
            dDLData.data.Add(config_Addresslevel2);
            var config_Addresslevel3 = await GetDDLAsync<config_AddressLevel3>("DDLAddressLevel3");
            dDLData.data.Add(config_Addresslevel3);

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


        public async Task SetDescription<T>(T model) where T : class
        {
            if (model == null) return;

            var mappings = new List<(int? Configid, string tableName, Action<string?> setter)>
            {
            (
                GetPropertyValue<int?>(model, "ClientTypeConfigId"),
                "config_ClientType",
                desc => SetPropertyValue(model, "ClientType", desc)
            ),
            (
                GetPropertyValue<int?>(model, "ClientSubTypeConfigId"),
                "config_ClientSubType",
                desc => SetPropertyValue(model, "ClientSubType", desc)
            ),
            (
                GetPropertyValue<int?>(model, "StatusConfigId"),
                "config_ClientStatus",
                desc => SetPropertyValue(model, "Status", desc)
            ),
            (
                GetPropertyValue<int?>(model, "GenderConfigId"),
                "config_Gender",
                desc => SetPropertyValue(model, "Gender", desc)
            ),
            (
                GetPropertyValue<int?>(model, "MaritalStatusConfigId"),
                "config_MaritalStatus",
                desc => SetPropertyValue(model, "MaritalStatus", desc)
            ),
            (
                GetPropertyValue<int?>(model, "configRelationshipId"),
                "config_Relationship",
                desc => SetPropertyValue(model, "configRelationship", desc)
            ),
            (
                GetPropertyValue<int?>(model, "configDocumentMasterId"),
                "config_DocumentMaster",
                desc => SetPropertyValue(model, "configDocumentMaster", desc)
            ),
            (
                GetPropertyValue<int?>(model, "configAddressLevel1Id"),
                "config_AddressLevel1",
                desc => SetPropertyValue(model, "configAddressLevel1", desc)
            ),
            (
                GetPropertyValue<int?>(model, "configAddressLevel2Id"),
                "config_AddressLevel2",
                desc => SetPropertyValue(model, "configAddressLevel2", desc)
            ),
            (
                GetPropertyValue<int?>(model, "configAddressLevel3Id"),
                "config_AddressLevel3",
                desc => SetPropertyValue(model, "configAddressLevel3", desc)
            ),
            (
                GetPropertyValue<int?>(model, "ServiceConfigId"),
                "config_Service",
                desc => SetPropertyValue(model, "ConfigService", desc)
            ),
            (
                GetPropertyValue<int?>(model, "MatterTypeConfigId"),
                "config_MatterType",
                desc => SetPropertyValue(model, "ConfigMatterType", desc)
            ),
            (
                GetPropertyValue<int?>(model, "MatterSubTypeConfigId"),
                "config_MatterSubType",
                desc => SetPropertyValue(model, "ConfigMatterSubType", desc)
            ),
            (
                GetPropertyValue<int?>(model, "ContactModeConfigId"),
                "config_ContactMode",
                desc => SetPropertyValue(model, "ConfigContactMode", desc)
            ),
            (
                GetPropertyValue<int?>(model, "ServiceStatusConfigId"),
                "config_ServiceStatus",
                desc => SetPropertyValue(model, "ConfigServiceStatus", desc)
            ),//
            (
                GetPropertyValue<int?>(model, "configDesignationId"),
                "config_Designation",
                desc => SetPropertyValue(model, "ConfigDesignation", desc)
            ),
            (
                GetPropertyValue<int?>(model, "configSpecializationId"),
                "config_Specialization",
                desc => SetPropertyValue(model, "configSpecialization", desc)
            ),
            (
                GetPropertyValue<int?>(model, "configLegalOfficerStatusId"),
                "config_LegalOfficerStatus",
                desc => SetPropertyValue(model, "configLegalOfficerStatus", desc)
            ),
            (
                GetPropertyValue<int?>(model, "configIDTypeId"),
                "config_IDType",
                desc => SetPropertyValue(model, "configIDType", desc)
            )
            };

            foreach (var (Configid, tableName, setter) in mappings)
            {
                Console.WriteLine($"TableName: {tableName} | ConfigId: {Configid}");
                if (Configid == null || Configid == 0) continue;

                var desc = await GetDescriptionFromCache(tableName, Configid.Value);
                Console.WriteLine($"DESC RESULT: {desc}");
                setter(desc);
            }
        }

        private async Task<string?> GetDescriptionFromCache(string tableName, int Configid)
        {
            var cacheKey = tableName;

            if (!_memoryCache.TryGetValue(cacheKey,
                    out Dictionary<int, string>? configData) || configData == null)
            {
                // Cache miss — fetch entire table ONCE
                configData = tableName switch
                {
                    "config_ClientType" => await _dbContext.config_ClientType
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_ClientSubType" => await _dbContext.config_ClientSubType
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_ClientStatus" => await _dbContext.config_ClientStatus
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_Gender" => await _dbContext.config_Gender
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_MaritalStatus" => await _dbContext.config_MaritalStatus
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_Relationship" => await _dbContext.config_Relationship
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_DocumentMaster" => await _dbContext.config_DocumentMaster
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_AddressLevel1" => await _dbContext.config_AddressLevel1
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_AddressLevel2" => await _dbContext.config_AddressLevel2
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_AddressLevel3" => await _dbContext.config_AddressLevel3
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_Service" => await _dbContext.config_Service
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_MatterType" => await _dbContext.config_MatterType
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_MatterSubType" => await _dbContext.config_MatterSubType
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_ContactMode" => await _dbContext.config_ContactMode
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_ServiceStatus" => await _dbContext.config_ServiceStatus
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_Designation" => await _dbContext.config_Designation
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_Specialization" => await _dbContext.config_Specialization
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_LegalOfficerStatus" => await _dbContext.config_LegalOfficerStatus
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    "config_IDType" => await _dbContext.config_IDType
                        .ToDictionaryAsync(x => x.ConfigId, x => x.Description),

                    _ => new Dictionary<int, string>()
                };

                _memoryCache.Set(cacheKey, configData, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }

            return configData.TryGetValue(Configid, out var desc) ? desc : null;
        }

        // Helper — Get property value without hardcoding
        private TValue? GetPropertyValue<TValue>(object model, string propertyName)
        {
            var prop = model.GetType().GetProperty(propertyName);
            if (prop == null) return default;

            var value = prop.GetValue(model);
            if (value == null) return default;

            // Handle Nullable types explicitly
            var targetType = typeof(TValue);
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            //                   ↑
            //   If TValue is int?  → returns int
            //   If TValue is int   → returns null → falls back to int

            return (TValue)Convert.ChangeType(value, underlyingType);
        }

        // Helper — Set property value without hardcoding
        private void SetPropertyValue(object model, string propertyName, string? value)
        {
            var prop = model.GetType().GetProperty(propertyName);
            prop?.SetValue(model, value);
        }
    }
}

