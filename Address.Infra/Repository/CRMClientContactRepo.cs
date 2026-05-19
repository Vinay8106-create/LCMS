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

        //public async Task<CRMClientContact> GetCRMClientContactByClientContactId(long CRMClientContactId, bool isTracking = false)
        //{
        //    return await GetByIdAsync(CRMClientContactId, isTracking) ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);
        //}


        public async Task<CRMClientContact> GetCRMClientContactByClientContactId(
        long CRMClientContactId,
        bool isTracking = false)
        {
            var query = _dbContext.CRMClientContact
                .Include(c => c.ResidentialAddress)
                .Include(c => c.CommunicationAddress)
                .AsQueryable();

            // Respect the tracking flag
            if (!isTracking)
                query = query.AsNoTracking();

            var contact = await query
                .FirstOrDefaultAsync(c => c.Id == CRMClientContactId);

            return contact ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);
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
            var contacts = await _dbContext.CRMClientContact
                .Where(c => c.ClientId == clientId)
                .Join(
                    _dbContext.config_ClientStatus,
                    contact => contact.StatusConfigId,
                    status => status.Id,
                    (contact, status) => new {
                        Contact = contact,
                        StatusDescription = status.Description
                    }
                )
                .OrderByDescending(x => x.Contact.Id)
                .ToListAsync();

            var mappedContacts = contacts.Select(x => {
                var dto = _mapper.Map<CRMClientContactDto>(x.Contact);
                dto.StatusDescription = x.StatusDescription;
                return dto;
            }).ToList();

            return new CRMClientContactSectionDto
            {
                Items = mappedContacts
            };
        }
    }
}

