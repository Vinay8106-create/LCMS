using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Infra;
using LCMS.Dto;
using LCMS.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infra
{
    public class CRMClientDocumentRepo : Repository<CRMClientDocument>, ICRMClientDocumentRepo
    {
        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;
        public CRMClientDocumentRepo(LCMSDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<CRMClientDocument> GetCRMClientDocumentDetailById(long CRMClientContactDetailId, bool isTracking = false)
        {
            throw new NotImplementedException();
        }

        public Task<CRMClientDocument> InsertCRMClientDocumentDetailAsync(CRMClientDocument request)
        {
            throw new NotImplementedException();
        }

        public Task<CRMClientDocument> UpdateCRMClientDcumentDetailAsync(CRMClientDocument request)
        {
            throw new NotImplementedException();
        }

        public async Task<CRMClientDocumentSectionDto> GetAllDocumentsByClientIdAsync(long clientId)
        {
            // Step 1 - Fetch from DB
            var documents = await _dbContext.CRMClientDocument
                .Where(doc => doc.ClientId == clientId)
                .OrderByDescending(doc => doc.Id)
                .ToListAsync();

            // Step 2 - Map in memory
            var mappedList = _mapper.Map<List<CRMClientDocumentDto>>(documents);

            // Step 3 - Wrap and return
            return new CRMClientDocumentSectionDto
            {
                Items = mappedList
            };
        }
    }
}

