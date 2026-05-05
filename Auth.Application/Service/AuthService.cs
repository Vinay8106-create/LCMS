using Galaxy.Utility;
using ITGAcc.Integration.Application;
using ITGAccounts.Dto;
using ITGAdmin.DTO;
using LCMS.Dto;
using LCMS.S2SLogic;
using Microsoft.Extensions.Configuration;

namespace Auth.Application
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IS2SLogic _s2Slogic;
        private readonly IAccBaseCompanyService _baseCompanyService;

        public AuthService(IS2SLogic s2SLogic, IConfiguration configuration, IAccBaseCompanyService accBaseCompanyService)
        {
            _s2Slogic = s2SLogic;
            _configuration = configuration;
            _baseCompanyService = accBaseCompanyService;
        }

        // --------------------------------------------------------------------
        //  AUTHENTICATION
        // --------------------------------------------------------------------

        public async Task<LoginUser> AuthenticateUser(UserCredentialRequest request)
        {
            var loginUser = new LoginUser();

            // Authenticate with external service
            try
            {
                loginUser.user = await _s2Slogic.Admin.AuthenticateUser(request);
                if (loginUser.user?.Msg?.ErrorMessage?.Count > 0 || loginUser.user == null)
                    return loginUser;
            }
            catch (Exception EX)
            {
                throw EX;
            }

            // Get CompanyId from config
            long.TryParse(_configuration.GetConfigurationFromAppSettings("CompanyId"), out long companyId);

            // Fetch company details
            var companyInfo = await _baseCompanyService.GetAccCompanyDetailsByCompanyId(companyId);
            if (companyInfo == null)
                return loginUser;

            // Set company info into login response
            loginUser.CompanyId = companyInfo.CompanyId;
            loginUser.CompanyName = companyInfo.CompanyName;
            loginUser.CurrencyValue = companyInfo.CurrencyValue;
            loginUser.SymbolValue = companyInfo.SymbolValue;
            loginUser.NoOfDecimalPlace = companyInfo.NoOfDecimalPlace;
            loginUser.CurrencyValueName = companyInfo.CurrencyValueName;
            loginUser.DecimalValueName = companyInfo.DecimalValueName;
            loginUser.CommaSeparatorValue = companyInfo.CommaSeparatorValue;
            loginUser.BranchName = companyInfo.BranchName;
            loginUser.CompanyFiscalYearId = companyInfo.CompanyFiscalYearId;

            // Prepare JWT token payload
            var jwtUserData = new AccApplicationInfo
            {
                UserLoginId = loginUser.user.UserLoginId,
                UserSerialId = loginUser.user.Id,
                UserName = loginUser.user.FullName,
                EmployeeCode = loginUser.user.EmployeeCode,
                ClientCode = request.ClientCode,
                CompanyId = companyInfo.CompanyId,
                CompanyBranchId = companyInfo.CompanyBranchId,
                CompanyFiscalYearId = companyInfo.CompanyFiscalYearId,
                CurrencyValue = companyInfo.CurrencyValue
            };

            var userData = new UserData
            {
                Properties = JsonUtility.SerializeObject(jwtUserData),
                ClientCode = request.ClientCode,
                UserLoginID = loginUser.user.UserLoginId
            };

            // Generate and assign JWT
            loginUser.user.KeyToken = new JsonToken().GenerateJWT(userData);

            return loginUser;
        }

        // --------------------------------------------------------------------
        // AUTHORIZATION
        // --------------------------------------------------------------------

        public async Task<UserAccessResponse> ValidateUserAction(UserAccessRequest request)
        {
            var result = new UserAccessResponse
            {
                CanEdit = false,
                CanDelete = false,
                CanView = false
            };

            if (request.UserRights.Any(x => x.ViewName == "fullaccess"))
            {
                result.CanEdit = true;
                result.CanDelete = true;
                return result;
            }

            var access = request.UserRights.FirstOrDefault(x => x.ViewName == request.ViewName);
            if (access != null)
            {
                var permissions = access.ResourcePermissions.ToDictionary(r => r.ResourceName, r => r.Permission);

                result.CanEdit = permissions.GetValueOrDefault("canEdit") == "1";
                result.CanDelete = permissions.GetValueOrDefault("canDelete") == "1";
                result.CanView = !result.CanEdit && permissions.GetValueOrDefault("canView") == "1";
            }

            return result;
        }

        // --------------------------------------------------------------------
        //  TOKEN / AUTH HELPERS
        // --------------------------------------------------------------------

        public async Task<List<string>> GetToken() =>
            await _s2Slogic.Admin.GetToken();

        public async Task<UserCredentialResponse> ValidateUserName(UserCredentialRequest request) =>
            await _s2Slogic.Admin.ValidateUserName(request);
        public async Task<UserCredentialResponse> ValidateUserEmail(UserCredentialRequest request) =>
            await _s2Slogic.Admin.ValidateUserEmail(request);

        public async Task<UserCredentialResponse> ForgotUserId(UserCredentialRequest request) =>
            await _s2Slogic.Admin.ForgotUserId(request);

        public async Task<UserCredentialResponse> VerifyUserOtp(UserCredentialRequest request) =>
            await _s2Slogic.Admin.VerifyUserOtp(request);

        public async Task<UserCredentialResponse> ResendOTPForUser(UserCredentialRequest request) =>
            await _s2Slogic.Admin.ResendOTPForUser(request);
        public async Task<UserCredentialResponse> ResendOTPForUserName(UserCredentialRequest request) =>
           await _s2Slogic.Admin.ResendOTPForUserName(request);

        public async Task<UserCredentialResponse> ForgotUserPassword(UserCredentialRequest request) =>
            await _s2Slogic.Admin.ForgotUserPassword(request);

        public async Task<UserCredentialResponse> ForgotUserName(UserCredentialRequest request) =>
            await _s2Slogic.Admin.ForgotUserName(request);

        public async Task<UserCredentialResponse> GetUserDetailsFromUrl(UserCredentialRequest request) =>
            await _s2Slogic.Admin.GetUserDetailsFromUrl(request);

        public async Task<UserCredentialResponse> VerifyUserOtpAndExpiry(UserCredentialRequest request) =>
            await _s2Slogic.Admin.VerifyUserOtpAndExpiry(request);

        public async Task<UserCredentialResponse> ResetUserPassword(UserCredentialRequest request) =>
            await _s2Slogic.Admin.ResetUserPassword(request);
    }
}
