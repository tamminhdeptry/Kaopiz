using CET.Domain.Dtos;

namespace CET.Service.Interface
{
    public interface IAuthService
    {
        Task<UserTokenDTO> Login(LoginDTO loginDTO);
        Task<TokenResponseDTO> RefreshToken(RefreshTokenDTO tokenDTO);
        Task Logout(string refreshToken);
        Task<string> Register(RegisterDto registerDTO);
    }
}
