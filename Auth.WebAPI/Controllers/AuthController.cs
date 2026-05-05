using Auth.Application;
using Galaxy.Dto;
using ITGAdmin.DTO;
using LCMS.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Auth.WebAPI
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("AuthenticateUser")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "AuthenticateUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginUser))]
        public async Task<IActionResult> AuthenticateUser(UserCredentialRequest Request)
        {
            var response = await _authService.AuthenticateUser(Request);
            return this.Ok(response);
        }

        [HttpPost]
        [Route("ValidateUserAction")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "ValidateUserAction")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserAccessResponse))]
        public async Task<IActionResult> validateUserAction(UserAccessRequest Request)
        {
            var response = await _authService.ValidateUserAction(Request);
            return this.Ok(response);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("gettoken")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "gettoken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
        public async Task<IActionResult> GetToken()
        {
            var response = await _authService.GetToken();
            return this.Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("validateusername")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "ValidateUsername")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCredentialResponse))]
        public async Task<IActionResult> ValidateUsername(UserCredentialRequest userCredentialRequest)
        {
            var response = await _authService.ValidateUserName(userCredentialRequest);
            return this.Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ValidateUserEmail")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "ValidateUserEmail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCredentialResponse))]
        public async Task<IActionResult> ValidateUserEmail(UserCredentialRequest userCredentialRequest)
        {
            var response = await _authService.ValidateUserEmail(userCredentialRequest);
            return this.Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgotuserid")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "ForgotUserId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCredentialResponse))]
        public async Task<IActionResult> ForgotUserId(UserCredentialRequest userCredentialRequest)
        {
            var response = await _authService.ForgotUserId(userCredentialRequest);
            return this.Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("verifyuserotp")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "VerifyUserOtp")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCredentialResponse))]
        public async Task<IActionResult> VerifyUserOtp(UserCredentialRequest userCredentialRequest)
        {
            var response = await _authService.VerifyUserOtp(userCredentialRequest);
            return this.Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("verifyuserotpandexpiry")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "VerifyUserOtpAndExpiry")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCredentialResponse))]
        public async Task<IActionResult> VerifyUserOtpAndExpiry(UserCredentialRequest userCredentialRequest)
        {
            var response = await _authService.VerifyUserOtpAndExpiry(userCredentialRequest);
            return this.Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("resetuserpassword")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "ResetUserPassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCredentialResponse))]
        public async Task<IActionResult> ResetUserPassword(UserCredentialRequest userCredentialRequest)
        {
            var response = await _authService.ResetUserPassword(userCredentialRequest);
            return this.Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("resendotpforuser")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "ResendOTPForUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCredentialResponse))]
        public async Task<IActionResult> ResendOTPForUser(UserCredentialRequest userCredentialRequest)
        {
            var response = await _authService.ResendOTPForUser(userCredentialRequest);
            return this.Ok(response);
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("ResendOTPForUserName")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "ResendOTPForUserName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCredentialResponse))]
        public async Task<IActionResult> ResendOTPForUserName(UserCredentialRequest userCredentialRequest)
        {
            var response = await _authService.ResendOTPForUserName(userCredentialRequest);
            return this.Ok(response);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("getuserdetailsfromurl")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "GetUserDetailsFromUrl")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCredentialResponse))]
        public async Task<IActionResult> GetUserDetailsFromUrl(UserCredentialRequest userCredentialRequest)
        {
            var response = await _authService.GetUserDetailsFromUrl(userCredentialRequest);
            return this.Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("forgotuserpassword")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "ForgotUserPassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCredentialResponse))]
        public async Task<IActionResult> ForgotUserPassword(UserCredentialRequest userCredentialRequest)
        {
            var response = await _authService.ForgotUserPassword(userCredentialRequest);
            return this.Ok(response);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("ForgotUserName")]
        [SwaggerOperation(Tags = new[] { "Auth" }, Summary = "ForgotUserName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCredentialResponse))]
        public async Task<IActionResult> ForgotUserName(UserCredentialRequest userCredentialRequest)
        {
            var response = await _authService.ForgotUserName(userCredentialRequest);
            return this.Ok(response);
        }

    }
}
