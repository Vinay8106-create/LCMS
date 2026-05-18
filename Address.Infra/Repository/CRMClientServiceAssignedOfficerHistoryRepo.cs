using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Infra;
using Galaxy.Utility;
using LCMS.Constants;
using LCMS.Domain;
using LCMS.Dto;
using LCMS.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CRM.Infra
{
    public class CRMClientServiceAssignedOfficerHistoryRepo : Repository<CRMClientServiceAssignedOfficer>, ICRMClientServiceAssignedOfficerHistoryRepo
    {
        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;
        public CRMClientServiceAssignedOfficerHistoryRepo(LCMSDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public SemaphoreSlim _referencenoLoack = new SemaphoreSlim(1, 1);

        public async Task<List<CRMClientServiceAssignedOfficerHistoryDto>> GetClientServiceAssignedOfficerHistoryById(long clientServiceId, bool isTracking = false)
        {
            List<CRMClientServiceAssignedOfficerHistoryDto> list = new List<CRMClientServiceAssignedOfficerHistoryDto>();
            var data = await _dbContext.CRMClientServiceAssignedOfficer
                            .AsNoTracking()
                            .Where(cssh => cssh.ClientServiceId == clientServiceId)
                            .ToListAsync();
            foreach (var item in data)
            {

                var dto = _mapper.Map<CRMClientServiceAssignedOfficerHistoryDto>(item);
                list.Add(dto);
            }

            return list;
        }

    }

}
