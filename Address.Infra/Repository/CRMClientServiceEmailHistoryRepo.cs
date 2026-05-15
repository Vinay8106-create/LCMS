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
    public class CRMClientServiceEmailHistoryRepo : Repository<CRMClientServiceEmailHistory>, ICRMClientServiceEmailHistoryRepo
    {
        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;
        public CRMClientServiceEmailHistoryRepo(LCMSDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public SemaphoreSlim _referencenoLoack = new SemaphoreSlim(1, 1);

        public async Task<List<CRMClientServiceEmailHistoryDto>> GetClientServiceEmailHistoryById(long clientServiceId, bool isTracking = false)
        {
            var data = await _dbContext.CRMClientServiceEmailHistory
    .AsNoTracking()
    .Where(cssh => cssh.ClientServiceId == clientServiceId)
    .ToListAsync();

            return _mapper.Map<List<CRMClientServiceEmailHistoryDto>>(data);
        }

    }

}
