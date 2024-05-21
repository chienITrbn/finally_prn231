using backend.Common;
using backend.version1.Domain.DTO;
using backend.version1.Domain.Entities;
using backend.version1.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace backend.version1.Controllers.Auth
{
    public class AuthenticationController : ApiBaseController<User, UserDto>
    {
        private readonly IAuthenticationService _authenService;

        public AuthenticationController(IAuthenticationService authenService, ILogger<ApiBaseController<User, UserDto>> logger) : base(authenService, logger)
        {
            _authenService = authenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _authenService.Register(request);
                return Ok(new SuccessResponse<UserDto>(user, "create successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _authenService.Login(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}