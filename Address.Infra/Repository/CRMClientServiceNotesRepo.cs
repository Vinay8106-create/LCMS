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
    public class CRMClientServiceNotesRepo : Repository<CRMClientServiceNotes>, ICRMClientServiceNotesRepo
    {
        private readonly LCMSDbContext _dbContext;
        private readonly IMapper _mapper;
        public CRMClientServiceNotesRepo(LCMSDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public SemaphoreSlim _referencenoLoack = new SemaphoreSlim(1, 1);

        public async Task<List<CRMClientServiceNotesDto>> GetClientServiceNotesById(long clientServiceId, bool isTracking = false)
        {
            List<CRMClientServiceNotesDto> list = new List<CRMClientServiceNotesDto>();
                        var data = await _dbContext.CRMClientServiceNotes
                .AsNoTracking()
                .Where(cssh => cssh.ClientServiceId == clientServiceId)
                .ToListAsync();
            foreach (var item in data)
            {

                var dto = _mapper.Map<CRMClientServiceNotesDto>(item);
                list.Add(dto);
            }

            return list;
        }

    }

}
