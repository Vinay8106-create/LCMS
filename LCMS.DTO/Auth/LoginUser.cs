using Galaxy.Dto;
using ITGAdmin.DTO;

namespace LCMS.Dto
{
    public class LoginUser
    {
        public UserDto user { get; set; } = new();
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CurrencyValue { get; set; }
        public string SymbolValue { get; set; }
        public int NoOfDecimalPlace { get; set; }
        public string CurrencyValueName { get; set; }
        public string DecimalValueName { get; set; }
        public string? CommaSeparatorValue { get; set; }
        public long CompanyBranchId { get; set; }
        public string BranchValue { get; set; }
        public string BranchName { get; set; }
        public long CompanyFiscalYearId { get; set; }
        public AppMessage? Msg { get; set; } = new AppMessage();
    }
}
