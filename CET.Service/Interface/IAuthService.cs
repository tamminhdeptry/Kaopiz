using CET.Domain.Dtos;

namespace CET.Service.Interface
{
    public interface IAuthService
    {
        Task<UserTokenDTO> Login(LoginDTO loginDTO, string userAgent);
        Task<TokenResponseDTO> RefreshToken(RefreshTokenDTO tokenDTO, string userAgent);
        Task Logout(string userAgent, string token);
        Task<string> Register(RegisterDto registerDTO);
    }
}
