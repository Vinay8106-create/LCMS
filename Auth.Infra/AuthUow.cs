using Auth.Application;
using AutoMapper;
using Galaxy.Domain.Models;
using Galaxy.Infra;
using ITGAcc.Integration.Application;
using ITGAcc.Integration.Infra;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infra {
    public class AuthUow : ITGUow, IAuthUow {
        protected readonly AuthDBContext _Context;
        protected readonly IMapper _Mapper;

        public AuthUow(AuthDBContext context, BaseRequestProfile requestProfile, IServiceProvider serviceProvider)
            : base(context, requestProfile) {
            _Context = context;
            _Mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        // Company Repositories
        private IAccCompanyOtherDetailRepo _accCompanyOtherDetailRepo;
        private IAccCompanyRepo _accCompanyRepo;
        private IAccCompanyBranchRepo _accCompanyBranchRepo;
        private IAccCompanyFiscalYearRepo _accCompanyFiscalYearRepo;

        // Ledger Transaction Repositories
        private IAccLedgerTransactionRepo _accLedgerTransactionRepo;
        private IAccLedgerTransactionSearchRepo _accLedgerTransactionSearchRepo;
        private IAccLedgerTransactionDetailRepo _accLedgerTransactionDetailRepo;
        private IAccLedgerTransactionDetailAttributeRepo _accLedgerTransactionDetailAttributeRepo;

        // Tax and Exchange Rate
        private IAccTaxRepo _accTaxRepo;
        private IAccTaxHistoryRepo _accTaxHistoryRepo;
        private IAccExchangeRateRepo _accExchangeRateRepo;

        // Ledger and Group
        private IAccLedgerRepo _accLedgerRepo;
        private IAccGroupRepo _accGroupRepo;
        private IAccLedgerBranchAccessRepo _accLedgerBranchAccessRepo;
        public IAccLedgerTransactionSearchWithRBRepo _AccLedgerTransactionSearchWithRBRepo;
        // Company
        public IAccCompanyOtherDetailRepo AccCompanyOtherDetailRepo => _accCompanyOtherDetailRepo ??= new AccCompanyOtherDetailRepo(_Context, _Mapper);
        public IAccCompanyRepo AccCompanyRepo => _accCompanyRepo ??= new AccCompanyRepo(_Context, _Mapper);
        public IAccCompanyBranchRepo AccCompanyBranchRepo => _accCompanyBranchRepo ??= new AccCompanyBranchRepo(_Context, _Mapper);
        public IAccCompanyFiscalYearRepo AccCompanyFiscalYearRepo => _accCompanyFiscalYearRepo ??= new AccCompanyFiscalYearRepo(_Context, _Mapper);

        // Ledger Transaction
        public IAccLedgerTransactionRepo AccLedgerTransactionRepo => _accLedgerTransactionRepo ??= new AccLedgerTransactionRepo(_Context, _Mapper);
        public IAccLedgerTransactionSearchRepo AccLedgerTransactionSearchRepo => _accLedgerTransactionSearchRepo ??= new AccLedgerTransactionSearchRepo(_Context, _Mapper);
        public IAccLedgerTransactionDetailRepo AccLedgerTransactionDetailRepo => _accLedgerTransactionDetailRepo ??= new AccLedgerTransactionDetailRepo(_Context, _Mapper);
        public IAccLedgerTransactionDetailAttributeRepo AccLedgerTransactionDetailAttributeRepo => _accLedgerTransactionDetailAttributeRepo ??= new AccLedgerTransactionDetailAttributeRepo(_Context, _Mapper);

        // Tax and Exchange
        public IAccTaxRepo AccTaxRepo => _accTaxRepo ??= new AccTaxRepo(_Context, _Mapper);
        public IAccTaxHistoryRepo AccTaxHistoryRepo => _accTaxHistoryRepo ??= new AccTaxHistoryRepo(_Context, _Mapper);
        public IAccExchangeRateRepo AccExchangeRateRepo => _accExchangeRateRepo ??= new AccExchangeRateRepo(_Context, _Mapper);

        // Ledger and Group
        public IAccLedgerRepo AccLedgerRepo => _accLedgerRepo ??= new AccLedgerRepo(_Context, _Mapper);
        public IAccGroupRepo AccGroupRepo => _accGroupRepo ??= new AccGroupRepo(_Context, _Mapper);
        public IAccLedgerBranchAccessRepo AccLedgerBranchAccessRepo => _accLedgerBranchAccessRepo ??= new AccLedgerBranchAccessRepo(_Context, _Mapper);

        public IAccLedgerTransactionSearchWithRBRepo AccLedgerTransactionSearchWithRBRepo => _AccLedgerTransactionSearchWithRBRepo ??= new AccLedgerTransactionSearchWithRBRepo(_Context, _Mapper);

    }
}
