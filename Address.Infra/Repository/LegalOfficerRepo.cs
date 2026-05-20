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

        //public async Task<LegalOfficerDto> GetLegalOfficerById(long LegalOfficerId, bool isTracking = false)
        //{
        //    //var legalOfficer = await GetByIdAsync(LegalOfficerId, isTracking) ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);

        //    //return _mapper.Map<LegalOfficerDto>(legalOfficer);

        //    IQueryable<LegalOfficer> query = _dbContext.LegalOfficer
        //    .Include(lo => lo.Photo)
        //    .Include(lo => lo.Doc)
        //    .Include(lo => lo.ResidentialAddress);

        //    if (!isTracking) query = query.AsNoTracking();

        //    var legalOfficer = await query.FirstOrDefaultAsync(lo => lo.Id == LegalOfficerId) ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);
        //    return _mapper.Map<LegalOfficerDto>(legalOfficer);
        //}

        public async Task<LegalOfficerDto> GetLegalOfficerById(long legalOfficerId, bool isTracking = false)
        {
            var legalOfficer = await _dbContext.LegalOfficer
            .Include(x => x.Photo)
            .Include(x => x.Doc)
            .FirstOrDefaultAsync(x => x.Id == legalOfficerId)
            ?? throw new BusinessException("Id not found", HttpStatusCode.NotFound);

            return _mapper.Map<LegalOfficerDto>(legalOfficer);
        }


        public async Task<DDL> getAllITGUser()
        {
            var data = await _dbContext.Set<User>()
                .Select(x => new DDLClass
                {
                    Id = EF.Property<long>(x, "Id"),
                    Constant = x.ContactNumber,
                    FilterKey = x.EmailId,
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
                    Description = u.EmailId,
                    FilterKey = (u.FirstName ?? "") + " " +
                      (u.MiddleName ?? "") + " " +
                      (u.LastName ?? "")
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }



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
            var addresslevel1 = await GetDDLAsync<config_Addresslevel1>("DDLAddresslevel1");
            dDLData.data.Add(addresslevel1);
            var addresslevel2 = await GetDDLAsync<config_Addresslevel2>("DDLAddresslevel2");
            dDLData.data.Add(addresslevel2);
            var User = await getAllLegalOfficer();
            dDLData.data.Add(User);
            var ITGUser = await getAllITGUser();
            dDLData.data.Add(ITGUser);

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
        public async Task<string> SetUserName(long userSerialId)
        {
            string name = "";
            var data = await (from lo in _dbContext.LegalOfficer
                              join u in _dbContext.Set<User>() on lo.UserSerialId equals u.Id
                              select new DDLClass
                              {
                                  Description = (u.FirstName ?? "") + " " +
                                                (u.MiddleName ?? "") + " " +
                                                (u.LastName ?? "")
                              })
                                .AsNoTracking()
                                .FirstOrDefaultAsync();
            name = data.Description;
            return name;

        }


        public async Task<List<LegalOfficerAppoinment>> GetAppointmentsAsync(long legalOfficerId, int year, int month)
        {
            return await _dbContext.LegalOfficerAppoinment
                .Where(a =>
                    a.LegalOfficerId == legalOfficerId &&
                    a.AppoinmentDate.Year == year &&
                    a.AppoinmentDate.Month == month)
                .OrderBy(a => a.AppoinmentDate)
                .ToListAsync();
        }

        public async Task<List<LegalOfficerBlockedDates>> GetBlockedDatesAsync(long legalOfficerId, int year, int month)
        {
            return await _dbContext.LegalOfficerBlockedDates
                .Where(b =>
                    b.LegalOfficerId == legalOfficerId &&
                    b.BlockDate.Year == year &&
                    b.BlockDate.Month == month)
                .OrderBy(b => b.BlockDate)
                .ToListAsync();
        }
    }
}

