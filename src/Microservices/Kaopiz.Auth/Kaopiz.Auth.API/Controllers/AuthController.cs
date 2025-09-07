using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Kaopiz.Shared.Contracts;
using Kaopiz.Auth.Application;

namespace Kaopiz.Auth.API
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiVersion("1.0")]
    public class AuthController : APIBaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IHttpContextAccessor accessor,
            IAuthService authService) : base(accessor)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ApiResponse<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var result = await _authService.LoginAsync(loginRequestDto);
            return result;
        }

        [HttpPost("register")]
        public async Task<ApiResponse<RegisterResponseDto>> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var result = await _authService.RegisterAsync(registerRequestDto);
            return result;
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task Logout()
        {
            await _authService.LogoutAsync();
        }
    }
}