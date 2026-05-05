using Galaxy.Infra;
using Galaxy.Utility;
using ITGAccounts.Constants;

namespace LCMS.Utility
{
    public class RefNoWrapper : IRefNoWrapper
    {
        private readonly ITGDbContext _context;

        private static readonly SemaphoreSlim _applicationRefNoLock = new(1, 1);
        public RefNoWrapper(ITGDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateRefNo(string configValue)
        {
            if (string.IsNullOrWhiteSpace(configValue)) return string.Empty;

            try
            {
                await _applicationRefNoLock.WaitAsync();

                using var reader = await _context.ExecuteSpAsync("SP",
                new
                {
                    //ConfigId = LCMSCommonConstants.ReferenceNumber.Id,

                    //ConfigConstant = configValue,

                    //RefMetaDataName = LmsCommonConstants.ReferenceNumber.MetadataName,

                    //PrefixAliceName = LmsCommonConstants.ReferenceNumber.PrefixName
                });

                var result = await reader.ReadAsync();
                string? refNo = result.SingleOrDefault()?.RefNo;

                return refNo ?? string.Empty;
            }
            catch (Exception ex)
            {
                ErrorLog.Instance.WriteErrorLog(ex);
                throw;
            }

            finally { _applicationRefNoLock.Release(); }
        }
    }
}
