using Common.HttpClient;
using Galaxy.Domain.Exceptions;
using Galaxy.Domain.Models;
using Galaxy.Dto;
using Galaxy.Utility;
using ITGAdmin.DTO;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace LCMS.S2SLogic
{
    public class AdminS2SLogic
    {
        private readonly string AdminEndPoint = "AdminEndPoint";
        public readonly IHttpServiceClient _IHttpServiceClient;
        public readonly HttpServiceRequest _HttpServiceRequest;

        public AdminS2SLogic(IHttpServiceClient httpServiceClient, BaseRequestProfile baseRequestProfile, IConfiguration configuration)
        {
            _IHttpServiceClient = httpServiceClient;
            _HttpServiceRequest = new HttpServiceRequest
            {
                ServiceEndpoint = AdminEndPoint,
                AccessToken = baseRequestProfile.Token,
                TenantCode = baseRequestProfile.TenantCode,
                CorrelationId = baseRequestProfile.CorrelationId,
                IsAuthorized = true,
                IsMutiTenentSolution = Convert.ToBoolean(configuration.GetConfigurationFromAppSettings("IsMutiTenentSolution"))
            };
        }

        public async Task<bool> IsUserBasedOnConfiguredGroup(string userLoginId, string groupName)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/External/IsUserBasedOnConfiguredGroup";
                _HttpServiceRequest.QueryString = $"?userLoginId={Uri.EscapeDataString(userLoginId)}&groupName={Uri.EscapeDataString(groupName)}";

                var result = await _IHttpServiceClient.GetAsync<bool>(_HttpServiceRequest);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ValidateUserInConfiguredGroup(int ConfigId, string ConfigConst, string MetadataName, long UserSerialId)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/External/ValidateUserInConfiguredGroup";
                _HttpServiceRequest.QueryString = $"?ConfigId={ConfigId}&ConfigConst={Uri.EscapeDataString(ConfigConst)}&MetadataName={Uri.EscapeDataString(MetadataName)}&UserSerialId={UserSerialId}";
                return await _IHttpServiceClient.GetAsync<bool>(_HttpServiceRequest);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Admin Service Is Down. Please Try Again Later", HttpStatusCode.ServiceUnavailable);
            }
        }

        public async Task<UserDto> LoadUserInformationByUserLoginId(string UserLoginId)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/External/LoadUserInformationByUserLoginID";
                _HttpServiceRequest.QueryString = $"?UserLoginID={Uri.EscapeDataString(UserLoginId)}";
                return await _IHttpServiceClient.GetAsync<UserDto>(_HttpServiceRequest) ?? new UserDto();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<DDLClass>> GetUsersBasedOnConfiguredGroup(int ConfigId, string ConfigConst, string MetaDataName)
        {
            _HttpServiceRequest.RoutePath = "/api/External/GetUsersBasedOnConfiguredGroup";
            _HttpServiceRequest.QueryString = $"?ConfigId={ConfigId}&ConfigConst={Uri.EscapeDataString(ConfigConst)}&MetadataName={Uri.EscapeDataString(MetaDataName)}";
            return await _IHttpServiceClient.GetAsync<List<DDLClass>>(_HttpServiceRequest) ?? new List<DDLClass>();
        }

        public async Task<List<DDLClass>> LoadAllUserByBranchAndConfigValues(string ConfigConst, int ConfigId, string metadataName, string UserLogin, string branch)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/External/LoadAllUserByBranchAndConfigValues";
                _HttpServiceRequest.QueryString =
                    $"?ConfigConst={Uri.EscapeDataString(ConfigConst)}" +
                    $"&ConfigId={(ConfigId)}" +
                    $"&metadataName={Uri.EscapeDataString(metadataName)}" +
                    $"&UserLogin={Uri.EscapeDataString(UserLogin)}" +
                    $"&branch={Uri.EscapeDataString(branch)}";
                var result = await _IHttpServiceClient.GetAsync<List<DDLClass>>(_HttpServiceRequest) ?? new List<DDLClass>();
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<DDLClass>> LoadAllBranchesDDLByUserSerialID(long userSerialId)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/External/LoadAllBranchesDDLByUserSerialID";
                _HttpServiceRequest.QueryString = $"?userSerialId={(userSerialId)}";

                var result = await _IHttpServiceClient.GetAsync<List<DDLClass>>(_HttpServiceRequest) ?? new List<DDLClass>();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserDto> AuthenticateUser(UserCredentialRequest request)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/Authentication/AuthenticateUser";
                _HttpServiceRequest.BodyPayload = request;
                _HttpServiceRequest.IsAuthorized = false;

                return await _IHttpServiceClient.PostAsync<UserDto>(_HttpServiceRequest) ?? new UserDto();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Login Methods

        public async Task<List<string>> GetToken()
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/Authentication/gettoken";
                _HttpServiceRequest.IsAuthorized = false;

                var result = await _IHttpServiceClient.GetAsync<List<string>>(_HttpServiceRequest) ?? new List<string>();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserCredentialResponse> ValidateUserName(UserCredentialRequest userCredentialRequest)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/Authentication/ValidateUsername";
                _HttpServiceRequest.BodyPayload = userCredentialRequest;
                _HttpServiceRequest.IsAuthorized = false;

                var result = await _IHttpServiceClient.PostAsync<UserCredentialResponse>(_HttpServiceRequest) ?? new UserCredentialResponse();

                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<UserCredentialResponse> ValidateUserEmail(UserCredentialRequest userCredentialRequest)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/Authentication/ValidateUserEmail";
                _HttpServiceRequest.BodyPayload = userCredentialRequest;
                _HttpServiceRequest.IsAuthorized = false;

                var result = await _IHttpServiceClient.PostAsync<UserCredentialResponse>(_HttpServiceRequest) ?? new UserCredentialResponse();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserCredentialResponse> ForgotUserId(UserCredentialRequest userCredentialRequest)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/Authentication/ForgotUserId";
                _HttpServiceRequest.BodyPayload = userCredentialRequest;
                _HttpServiceRequest.IsAuthorized = false;

                var result = await _IHttpServiceClient.PostAsync<UserCredentialResponse>(_HttpServiceRequest) ?? new UserCredentialResponse();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserCredentialResponse> VerifyUserOtp(UserCredentialRequest userCredentialRequest)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/Authentication/VerifyUserOtp";
                _HttpServiceRequest.BodyPayload = userCredentialRequest;
                _HttpServiceRequest.IsAuthorized = false;

                var result = await _IHttpServiceClient.PostAsync<UserCredentialResponse>(_HttpServiceRequest) ?? new UserCredentialResponse();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserCredentialResponse> VerifyUserOtpAndExpiry(UserCredentialRequest userCredentialRequest)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/Authentication/VerifyUserOtpAndExpiry";
                _HttpServiceRequest.BodyPayload = userCredentialRequest;
                _HttpServiceRequest.IsAuthorized = false;

                var result = await _IHttpServiceClient.PostAsync<UserCredentialResponse>(_HttpServiceRequest) ?? new UserCredentialResponse();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserCredentialResponse> ResetUserPassword(UserCredentialRequest userCredentialRequest)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/Authentication/ResetUserPassword";
                _HttpServiceRequest.BodyPayload = userCredentialRequest;
                _HttpServiceRequest.IsAuthorized = false;

                var result = await _IHttpServiceClient.PostAsync<UserCredentialResponse>(_HttpServiceRequest) ?? new UserCredentialResponse();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserCredentialResponse> ResendOTPForUser(UserCredentialRequest userCredentialRequest)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/Authentication/ResendOTPForUser";
                _HttpServiceRequest.BodyPayload = userCredentialRequest;
                _HttpServiceRequest.IsAuthorized = false;

                var result = await _IHttpServiceClient.PostAsync<UserCredentialResponse>(_HttpServiceRequest) ?? new UserCredentialResponse();

                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<UserCredentialResponse> ResendOTPForUserName(UserCredentialRequest userCredentialRequest)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/Authentication/ResendOTPForUserName";
                _HttpServiceRequest.BodyPayload = userCredentialRequest;
                _HttpServiceRequest.IsAuthorized = false;

                var result = await _IHttpServiceClient.PostAsync<UserCredentialResponse>(_HttpServiceRequest) ?? new UserCredentialResponse();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserCredentialResponse> GetUserDetailsFromUrl(UserCredentialRequest userCredentialRequest)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/Authentication/GetUserDetailsFromUrl";
                _HttpServiceRequest.BodyPayload = userCredentialRequest;
                _HttpServiceRequest.IsAuthorized = false;

                var result = await _IHttpServiceClient.PostAsync<UserCredentialResponse>(_HttpServiceRequest) ?? new UserCredentialResponse();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserCredentialResponse> ForgotUserPassword(UserCredentialRequest userCredentialRequest)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/Authentication/ForgotUserPassword";
                _HttpServiceRequest.BodyPayload = userCredentialRequest;
                _HttpServiceRequest.IsAuthorized = false;

                var result = await _IHttpServiceClient.PostAsync<UserCredentialResponse>(_HttpServiceRequest) ?? new UserCredentialResponse();

                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<UserCredentialResponse> ForgotUserName(UserCredentialRequest userCredentialRequest)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/Authentication/ForgotUserName";
                _HttpServiceRequest.BodyPayload = userCredentialRequest;
                _HttpServiceRequest.IsAuthorized = false;

                var result = await _IHttpServiceClient.PostAsync<UserCredentialResponse>(_HttpServiceRequest) ?? new UserCredentialResponse();

                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        public async Task<List<DDLClass>> GetCurrencyDDL()
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/External/GetCurrencyValueDDL";
                return await _IHttpServiceClient.GetAsync<List<DDLClass>>(_HttpServiceRequest) ?? new List<DDLClass>();
            }
            catch
            {
                return new List<DDLClass>();
            }
        }

        public async Task<List<DDLClass>> GetConfigValueDDLByConfigID(int ConfigId)
        {
            try
            {
                _HttpServiceRequest.RoutePath = "/api/External/GetConfigValueDDLByConfigID";
                _HttpServiceRequest.QueryString = $"?ConfigId={ConfigId}";

                return await _IHttpServiceClient.GetAsync<List<DDLClass>>(_HttpServiceRequest) ?? new List<DDLClass>();
            }
            catch
            {
                throw;
            }
        }
    }
}