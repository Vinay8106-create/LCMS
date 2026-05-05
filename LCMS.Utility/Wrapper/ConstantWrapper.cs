using Galaxy.Domain.Models;
using Galaxy.Utility;

namespace LCMS.Utility
{
    public static class ConstantWrapper
    {
        public static string GetFullName(User? user) =>
           user == null ? string.Empty : string.Join(" ", new[] { user.FirstName, user.MiddleName, user.LastName }
           .Where(s => !string.IsNullOrEmpty(s)));

        public static string GetDescription(ConfigValue? ConfigValue)
        {
            try
            {
                var Desc = ConfigValue == null ? string.Empty : ConfigValue?.Description ?? string.Empty;
                return Desc;
            }
            catch (Exception ex)
            {
                ErrorLog.Instance.WriteErrorLog(ex.Message);
                throw;
            }
        }
    }
}
