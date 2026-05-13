using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Infra;
using LCMS.Dto;
using LCMS.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Reflection;

namespace CRM.Infra
{
    public class CRMClientDocumentRepo : Repository<CRMClientDocument>, ICRMClientDocumentRepo
    {
        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        public CRMClientDocumentRepo(LCMSDbContext dbContext, IMapper mapper, IMemoryCache memoryCache) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public Task<CRMClientDocument> GetCRMClientDocumentDetailById(long CRMClientContactDetailId, bool isTracking = false)
        {
            throw new NotImplementedException();
        }

        public async Task<CRMClientDocument> InsertCRMClientDocumentAsync(CRMClientDocument request)
        {
            await AddAsync(request);

            return request;
        }

        public async Task<CRMClientDocument> UpdateCRMClientDocumentAsync(CRMClientDocument request)
        {
            var clientDocument = await _dbContext.CRMClientDocument
                                        .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (clientDocument == null)
                throw new BusinessException("Id not found", HttpStatusCode.BadRequest);
            _mapper.Map(request, clientDocument);

            return clientDocument;
        }

        public async Task<CRMClientDocumentSectionDto> GetAllDocumentsByClientIdAsync(long clientId)
        {
            var documents = await _dbContext.CRMClientDocument
                .Include(x => x.Document)
                .Where(x => x.ClientId == clientId)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            var documentDtos = _mapper.Map<List<CRMClientDocumentDto>>(documents);

            // SetDescription on DTOs, not entities
            foreach (var dto in documentDtos)
            {
                await SetDescription(dto);
            }

            return new CRMClientDocumentSectionDto
            {
                Items = documentDtos  // Use the same list, not re-mapped
            };
        }

        public async Task SetDescription<T>(T model) where T : class
        {
            if (model == null) return;

            var mappings = new List<(int? Configid, string tableName, Action<string?> setter)>
            {
            (
                GetPropertyValue<int?>(model, "DocumentMasterId"),
                "config_DocumentMaster",
                desc => SetPropertyValue(model, "docname", desc)
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
                    "config_DocumentMaster" => await _dbContext.config_DocumentMaster
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
            var prop = model.GetType().GetProperty(
                propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase  // ✅ case-insensitive
            );
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
            var prop = model.GetType().GetProperty(
                propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase  // ✅ case-insensitive
            );
            prop?.SetValue(model, value);
        }
    }
}

