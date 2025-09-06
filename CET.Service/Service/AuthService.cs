using CET.Domain.Dtos;
using CET.Service.Interface;

namespace CET.Service.Service
{
    public class AuthService : IAuthService
    {
        public Task<UserTokenDTO> Login(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }

        public Task Logout(string token)
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponseDTO> RefreshToken(RefreshTokenDTO tokenDTO)
        {
            throw new NotImplementedException();
        }

        public Task<string> Register(RegisterDto registerDTO)
        {
            throw new NotImplementedException();
        }
    }
}
