using ITGAdmin.DTO;
using LCMS.Dto;

namespace Auth.Application
{
    public interface IAuthService
    {
        // --- Authentication & Access ---

        /// <summary>
        /// Authenticates a user using provided credentials.
        /// </summary>
        Task<LoginUser> AuthenticateUser(UserCredentialRequest request);

        /// <summary>
        /// Validates if the user has access to perform the requested action.
        /// </summary>
        Task<UserAccessResponse> ValidateUserAction(UserAccessRequest request);

        // --- Token Handling ---

        /// <summary>
        /// Retrieves a list of tokens (purpose/context-specific).
        /// </summary>
        Task<List<string>> GetToken();

        // --- Username and Password Management ---

        /// <summary>
        /// Validates the user’s credentials for login.
        /// </summary>
        Task<UserCredentialResponse> ValidateUserName(UserCredentialRequest request);
        Task<UserCredentialResponse> ValidateUserEmail(UserCredentialRequest request);

        /// <summary>
        /// Handles forgot user ID request.
        /// </summary>
        Task<UserCredentialResponse> ForgotUserId(UserCredentialRequest request);

        /// <summary>
        /// Verifies the OTP sent to the user.
        /// </summary>
        Task<UserCredentialResponse> VerifyUserOtp(UserCredentialRequest request);

        /// <summary>
        /// Resends OTP to the user.
        /// </summary>
        Task<UserCredentialResponse> ResendOTPForUser(UserCredentialRequest request);
        Task<UserCredentialResponse> ResendOTPForUserName(UserCredentialRequest request);

        /// <summary>
        /// Handles forgot password request for a user.
        /// </summary>
        Task<UserCredentialResponse> ForgotUserPassword(UserCredentialRequest request);
        Task<UserCredentialResponse> ForgotUserName(UserCredentialRequest request);

        /// <summary>
        /// Retrieves user details using a URL token or parameter.
        /// </summary>
        Task<UserCredentialResponse> GetUserDetailsFromUrl(UserCredentialRequest request);

        /// <summary>
        /// Verifies both OTP and its expiration.
        /// </summary>
        Task<UserCredentialResponse> VerifyUserOtpAndExpiry(UserCredentialRequest request);

        /// <summary>
        /// Resets the user’s password.
        /// </summary>
        Task<UserCredentialResponse> ResetUserPassword(UserCredentialRequest request);
    }
}
