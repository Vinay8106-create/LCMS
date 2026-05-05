using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Infra;
using LCMS.Dto;
using LCMS.Utility;
using CRM.Application;
using CRM.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using LCMS.Persistence;

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
    }
}

