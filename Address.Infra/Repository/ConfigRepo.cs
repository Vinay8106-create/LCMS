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
                    Id = EF.Property<int>(x, "Id"),
                    Description = EF.Property<string>(x, "Description")
                }).AsNoTracking().ToListAsync();

            return new DDL
            {
                Key = key,
                Value = data
            };
        }

        //public async Task SetDescription<T>(T model) where T : class
        //{
        //    var type = typeof(T);

        //    var idProp = type.GetProperty("ClientTypeConfigId");
        //    var descProp = type.GetProperty("ClientType");

        //    if (idProp == null || descProp == null) return;

        //    var value = idProp.GetValue(model);

        //    if (value == null) return;

        //    if (!int.TryParse(value.ToString(), out int id) || id == 0) return;

        //    var desc = await _dbContext.config_ClientType
        //        .Where(x => x.Id == id)
        //        .Select(x => x.Description)
        //        .FirstOrDefaultAsync();

        //    descProp.SetValue(model, desc);
        //}

        public async Task SetDescription<T>(T model) where T : class
        {
            if (model == null) return;

            var mappings = new List<(int? id, string tableName, Action<string?> setter)>
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
            )
            };

            foreach (var (id, tableName, setter) in mappings)
            {
                if (id == null || id == 0) continue;
                var desc = await GetDescriptionFromCache(tableName, id.Value);
                setter(desc);
            }
        }

        private async Task<string?> GetDescriptionFromCache(string tableName, long id)
        {
            var cacheKey = $"config_{tableName}";

            if (!_memoryCache.TryGetValue(cacheKey,
                    out Dictionary<long, string>? configData) || configData == null)
            {
                // Cache miss — fetch entire table ONCE
                configData = tableName switch
                {
                    "config_ClientType" => await _dbContext.config_ClientType
                        .ToDictionaryAsync(x => x.Id, x => x.Description),

                    "config_ClientSubType" => await _dbContext.config_ClientSubType
                        .ToDictionaryAsync(x => x.Id, x => x.Description),

                    "config_ClientStatus" => await _dbContext.config_ClientStatus
                        .ToDictionaryAsync(x => x.Id, x => x.Description),

                    "config_Gender" => await _dbContext.config_Gender
                        .ToDictionaryAsync(x => x.Id, x => x.Description),

                    "config_MaritalStatus" => await _dbContext.config_MaritalStatus
                        .ToDictionaryAsync(x => x.Id, x => x.Description),

                    "config_Relationship" => await _dbContext.config_Relationship
                        .ToDictionaryAsync(x => x.Id, x => x.Description),

                    _ => new Dictionary<long, string>()
                };

                _memoryCache.Set(cacheKey, configData, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }

            return configData.TryGetValue(id, out var desc) ? desc : null;
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

