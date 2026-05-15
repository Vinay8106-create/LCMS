using AutoMapper;
using CRM.Application;
using Galaxy.Domain.Models;
using Galaxy.Infra;
using LCMS.Persistence;
using Master.Infra;
using Microsoft.Extensions.Caching.Memory;

namespace CRM.Infra
{

    public class CRMUow(LCMSDbContext dbContext, BaseRequestProfile requestProfile, IMapper mapper, IServiceProvider provider, IMemoryCache memoryCache) : ITGUow(dbContext, requestProfile), ICRMUow
    {
        private IConfigRepo _configRepo;
        private IAddressRepo _addressRepo;
        private IDocumentFileRepo _DocumentFileRepo;
        private ICRMClientRepo _CRMClientRepo;
        private ICRMClientContactRepo _CRMClientContactRepo;
        private ICRMClientDocumentRepo _CRMClientDocumentRepo;
        private ICRMClientSearchRepo _CRMClientSearchRepo;
        private ICRMClientServiceRepo _CRMClientServiceRepo;
        private ICRMClientServiceSearchRepo _CRMClientServiceSearchRepo;
        private ICRMClientServiceStatusHistoryRepo _CRMClientServiceStatusHistoryRepo;
        private ICRMClientServiceAssignedOfficerHistoryRepo _CRMClientServiceAssignedOfficerHistoryRepo;
        private ICRMClientServiceEmailHistoryRepo _CRMClientServiceEmailHistoryRepo;
        private ICRMClientServiceNotesRepo _CRMClientServiceNotesRepo;
      

        private ILegalOfficerRepo _LegalOfficerRepo;

        private ILegalOfficerScheduleRepo _LegalOfficerScheduleRepo;
        private ILegalOfficerBlockDateRepo _LegalOfficerBlockDateRepo;
        private ILegalOfficerAppoinmentRepo _LegalOfficerAppoinmentRepo;

        private ILegalOfficerSearchRepo _LegalOfficerSearchRepo;
        private ILegalOfficerBlockedDateSearchRepo _LegalOfficerBlockedDateSearchRepo;


        public IConfigRepo ConfigRepo => _configRepo ?? new ConfigRepo(dbContext, mapper, memoryCache);
        public IAddressRepo AddressRepo => _addressRepo ?? new AddressRepo(dbContext, provider);

        public IDocumentFileRepo DocumentFileRepo => _DocumentFileRepo ?? new DocumentFileRepo(dbContext, provider);
        public ICRMClientRepo CRMClientRepo => _CRMClientRepo ??= new CRMClientRepo(dbContext, mapper);
        public ICRMClientContactRepo CRMClientContactRepo => _CRMClientContactRepo ??= new CRMClientContactRepo(dbContext, mapper);
        public ICRMClientDocumentRepo CRMClientDocumentRepo => _CRMClientDocumentRepo ??= new CRMClientDocumentRepo(dbContext, mapper, memoryCache);
        public ICRMClientSearchRepo CRMClientSearchRepo => _CRMClientSearchRepo ??= new CRMClientSearchRepo(dbContext);
        public ICRMClientServiceRepo CRMClientServiceRepo => _CRMClientServiceRepo ??= new CRMClientServiceRepo(dbContext, mapper);
        public ICRMClientServiceStatusHistoryRepo CRMClientServiceStatusHistoryRepo => _CRMClientServiceStatusHistoryRepo ??= new CRMClientServiceStatusHistoryRepo(dbContext, mapper);
        public ICRMClientServiceAssignedOfficerHistoryRepo CRMClientServiceAssignedOfficerHistoryRepo => _CRMClientServiceAssignedOfficerHistoryRepo ??= new CRMClientServiceAssignedOfficerHistoryRepo(dbContext, mapper);
        public ICRMClientServiceEmailHistoryRepo CRMClientServiceEmailHistoryRepo => _CRMClientServiceEmailHistoryRepo ??= new CRMClientServiceEmailHistoryRepo(dbContext, mapper);
        public ICRMClientServiceNotesRepo CRMClientServiceNotesRepo => _CRMClientServiceNotesRepo ??= new CRMClientServiceNotesRepo(dbContext, mapper);
        public ICRMClientServiceSearchRepo CRMClientServiceSearchRepo => _CRMClientServiceSearchRepo ??= new CRMClientServiceSearchRepo(dbContext);
        public ILegalOfficerRepo LegalOfficerRepo => _LegalOfficerRepo ??= new LegalOfficerRepo(dbContext, mapper);

        public ILegalOfficerScheduleRepo LegalOfficerScheduleRepo => _LegalOfficerScheduleRepo ??= new LegalOfficerScheduleRepo(dbContext, mapper);
        public ILegalOfficerBlockDateRepo LegalOfficerBlockDateRepo => _LegalOfficerBlockDateRepo ??= new LegalOfficerBlockDateRepo(dbContext, mapper);
        public ILegalOfficerAppoinmentRepo LegalOfficerAppoinmentRepo => _LegalOfficerAppoinmentRepo ??= new LegalOfficerAppoinmentRepo(dbContext, mapper);

        public ILegalOfficerSearchRepo LegalOfficerSearchRepo => _LegalOfficerSearchRepo ??= new LegalOfficerSearchRepo(dbContext);
        public ILegalOfficerBlockedDateSearchRepo LegalOfficerBlockedDateSearchRepo => _LegalOfficerBlockedDateSearchRepo ??= new LegalOfficerBlockedDateSearchRepo(dbContext);

    }
}
