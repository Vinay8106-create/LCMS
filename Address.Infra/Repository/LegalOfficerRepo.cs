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

        public async Task<LegalOfficerDto> GetLegalOfficerById(long LegalOfficerId, bool isTracking = false)
        {
            var legalOfficer = await GetByIdAsync(LegalOfficerId, isTracking) ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);

            return _mapper.Map<LegalOfficerDto>(legalOfficer);
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

        public async Task<DDLClass> GetDetailsFromITGUser(long UserSerialId)
        {
            return await _dbContext.Set<User>()
                .Where(u => u.Id == UserSerialId)
                .Select(u => new DDLClass
                {
                    Id = u.Id,
                    Constant = u.ContactNumber,
                    Description = u.EmailId
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        //public async Task<LegalOfficer> InsertLegalOfficer(LegalOfficer request)
        //{
        //    var isLegalOfficerExists = await _dbContext.LegalOfficer
        //        .AnyAsync(x => x.UserSerialId == request.UserSerialId);

        //    if (isLegalOfficerExists)
        //        throw new BusinessException($"A Legal Officer with UserLoginId '{request.UserSerialId}' already exists.");


        //    var legalOfficer = _mapper.Map<LegalOfficer>(request);
        //    await _dbContext.LegalOfficer.AddAsync(legalOfficer);
        //    await _dbContext.SaveChangesAsync();

        //    return legalOfficer;
        //}

        public async Task<LegalOfficer> InsertLegalOfficer(LegalOfficer legalOfficer)
        {
            var isLegalOfficerExists = await _dbContext.LegalOfficer
                .AnyAsync(x => x.UserSerialId == legalOfficer.UserSerialId);

            if (isLegalOfficerExists)
                throw new BusinessException($"A Legal Officer with UserLoginId '{legalOfficer.UserSerialId}' already exists.");

            if (legalOfficer.ResidentialAddress != null && legalOfficer.ResidentialAddress.Id > 0)
                _dbContext.Entry(legalOfficer.ResidentialAddress).State = EntityState.Unchanged;
            await AddAsync(legalOfficer);

            return legalOfficer;
        }

        public async Task<LegalOfficer> UpdateLegalOfficer(LegalOfficerDto request)
        {
            var legalOfficer = await _dbContext.LegalOfficer.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (legalOfficer == null)
                throw new BusinessException("Id not found", HttpStatusCode.BadRequest);
            _mapper.Map(request, legalOfficer);

            return legalOfficer;
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
            var User = await getAllLegalOfficer();
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
    }
}

