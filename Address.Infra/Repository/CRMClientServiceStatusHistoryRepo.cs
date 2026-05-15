using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Infra;
using Galaxy.Utility;
using ITGAccounts.Dto;
using LCMS.Constants;
using LCMS.Domain;
using LCMS.Dto;
using LCMS.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace CRM.Infra
{
    public class CRMClientServiceStatusHistoryRepo : Repository<CRMClientServiceStatusHistory>, ICRMClientServiceStatusHistoryRepo
    {
        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;
        public CRMClientServiceStatusHistoryRepo(LCMSDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public SemaphoreSlim _referencenoLoack = new SemaphoreSlim(1, 1);

        public async Task<List<CRMClientServiceStatusHistoryDto>> GetClientServiceStatusHistoryById(long clientServiceId, bool isTracking = false)
        {
            List<CRMClientServiceStatusHistoryDto> list =new List<CRMClientServiceStatusHistoryDto>();
            var data = await _dbContext.CRMClientServiceStatusHistory
    .AsNoTracking()
    .Where(cssh => cssh.ClientServiceId == clientServiceId)
    .ToListAsync();
            foreach (var item in data)
            {
                
                var dto = _mapper.Map<CRMClientServiceStatusHistoryDto>(item); 
                list.Add(dto);  
            }
            
            return list;
        }        
    }

}
