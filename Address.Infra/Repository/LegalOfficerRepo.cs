using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Domain.Models;
using Galaxy.Dto;
using Galaxy.Infra;
using LCMS.Dto;
using LCMS.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static Dapper.SqlMapper;

namespace CRM.Infra
{
    public class LegalOfficerRepo : Repository<LegalOfficer>, ILegalOfficerRepo
    {
        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;
        public LegalOfficerRepo(LCMSDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<LegalOfficer> GetLegalOfficerById(long LegalOfficerId, bool isTracking = false)
        {
            return await GetByIdAsync(LegalOfficerId, isTracking) ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);
        }

        public async Task<LegalOfficer> InsertLegalOfficer(LegalOfficerDto request)
        {
            var clientContact = _mapper.Map<LegalOfficer>(request);
            await AddAsync(clientContact);
            return clientContact;
        }

        public async Task<LegalOfficer> UpdateLegalOfficer(LegalOfficerDto request)
        {
            var clientContact = await _dbContext.LegalOfficer.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (clientContact == null)
                throw new BusinessException("Id not found", HttpStatusCode.BadRequest);
            _mapper.Map(request, clientContact);
            return clientContact;
        }

        public async Task<DDLData> GetLegalOfficerInitialData()
        {
            DDLData dDLData = new DDLData();
            var ServiceType = await GetDDLAsync<config_Designation>("DDLDesignation");
            dDLData.data.Add(ServiceType);
            var MatterType = await GetDDLAsync<config_Specialization>("DDLSpecialization");
            dDLData.data.Add(MatterType);
            var MatterSubType = await GetDDLAsync<config_LegalOfficerStatus>("DDLLegalOfficerStatus");
            dDLData.data.Add(MatterSubType);
            var ContactMode = await GetDDLAsync<config_IDType>("DDLIDType");
            dDLData.data.Add(ContactMode);
            var User = await getAllITGUser();
            dDLData.data.Add(User);

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

        public async Task<DDL> getAllITGUser()
        {
            var data = await _dbContext.Set<User>()
                .Select(x => new DDLClass
                {
                    Id = EF.Property<long>(x, "Id"),
                    Description = (x.FirstName ?? "") + " " +
                      (x.MiddleName ?? "") + " " +
                      (x.LastName ?? "")
                }).AsNoTracking().ToListAsync();

            return new DDL
            {
                Key = "DDLUser",
                Value = data
            };

        }
       
    }
}

