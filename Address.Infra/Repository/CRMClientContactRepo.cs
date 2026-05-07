using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Infra;
using LCMS.Dto;
using LCMS.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CRM.Infra
{
    public class CRMClientContactRepo : Repository<CRMClientContact>, ICRMClientContactRepo
    {
        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;
        public CRMClientContactRepo(LCMSDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CRMClientContact> GetCRMClientContactByClientContactId(long CRMClientContactId, bool isTracking = false)
        {
            return await GetByIdAsync(CRMClientContactId, isTracking) ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);
        }

        public async Task<CRMClientContact> InsertCRMClientContactAsync(CRMClientContactDto request)
        {
            var clientContact = _mapper.Map<CRMClientContact>(request);
            await AddAsync(clientContact);
            return clientContact;
        }

        public async Task<CRMClientContact> UpdateCRMClientContactAsync(CRMClientContactDto request)
        {
            var clientContact = await _dbContext.CRMClientContact.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (clientContact == null)
                throw new BusinessException("Id not found", HttpStatusCode.BadRequest);
            _mapper.Map(request, clientContact);
            return clientContact;
        }


        public async Task<CRMClientContactSectionDto> GetAllClientContactsByClientIdAsync(long clientId)
        {
            // Step 1 - Fetch entities from DB
            var contacts = await _dbContext.CRMClientContact
                .Where(c => c.ClientId == clientId)
                .OrderByDescending(c => c.Id)
                .ToListAsync();

            var mappedContacts = _mapper.Map<List<CRMClientContactDto>>(contacts);

            return new CRMClientContactSectionDto
            {
                Items = mappedContacts
            };
        }
    }
}

