using Microsoft.AspNetCore.Mvc;
using CET.Service.Interface;
using CET.Domain.Dtos;

namespace CET.API.Controllers
{
    [ApiVersion("1")]
    public class UserController : APIBaseController
    {
        private readonly IAuthService _authService;
        public UserController(IHttpContextAccessor accessor,
            IAuthService authService) : base(accessor)
        {
            _authService = authService;
        }

        [HttpPost("auth/login")]
        public async Task<UserTokenDTO> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _authService.Login(loginDTO);
            return result;
        }

        [HttpPost("auth/logout")]
        public async Task Logout([FromBody] RefreshTokenDTO tokenDTO)
        {
            await _authService.Logout(tokenDTO.RefreshToken);
        }

        [HttpPost("auth/refreshToken")]
        public async Task<TokenResponseDTO> RefreshToken([FromBody] RefreshTokenDTO tokenDTO)
        {
            var result = await _authService.RefreshToken(tokenDTO);
            return result;
        }

        [HttpPost("auth/register")]
        public async Task<string> Register([FromBody] RegisterDto userDTO)
        {
            var result = await _authService.Register(userDTO);
            return result;
        }
    }
}
