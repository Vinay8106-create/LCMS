using CRM.Application;
using Galaxy.Dto;
using LCMS.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CRM.WebAPI
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class ConfigController : ControllerBase
    {
        private readonly IConfigService _configService;

        public ConfigController(IConfigService configService)
        {
            _configService = configService;
        }

        #region Config

        [HttpGet("GetConfig")]
        [SwaggerOperation(Tags = new[] { "Config" }, Summary = "GetConfig")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConfigDto))]
        public async Task<IActionResult> GetConfig([FromQuery] long Id)
        {
            return this.Ok(await _configService.GetConfig(Id));
        }


        [HttpGet("CreateConfig")]
        [SwaggerOperation(Tags = new[] { "Config" }, Summary = "CreateConfig")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConfigDto))]
        public async Task<IActionResult> CreateConfig()
        {
            return this.Ok(await _configService.CreateConfig());
        }

        [HttpPost("SaveConfig")]
        [SwaggerOperation(Tags = new[] { "Config" }, Summary = "SaveConfig")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConfigDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AppMessage))]
        public async Task<IActionResult> SaveConfig(ConfigDto request)
        {
            return this.Ok(await _configService.SaveConfig(request));
        }

        #endregion
    }
}

