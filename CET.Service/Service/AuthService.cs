using CET.Domain.Caching;
using CET.Domain.Dtos;
using CET.Domain.Enum;
using CET.Infrastructure.Entity.Users;
using CET.Service.Interface;
using Newtonsoft.Json;

namespace CET.Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly ISecurityService _securityService;
        private readonly IUserCacheService _userCacheService;

        public AuthService(ISecurityService securityService, IUserCacheService userCacheService)
        {
            _securityService = securityService;
            _userCacheService = userCacheService;
        }

        public async Task<UserTokenDTO> Login(LoginDTO loginDTO)
        {
            var cache = await _userCacheService.FindUserCacheByUserName(loginDTO.UserName);
            if (cache == null)
            {
                throw new InvalidOperationException("Username không ton tai.");
            }

            loginDTO.Password = _securityService.ComputeSha256Hash(loginDTO.Password);
            if (!loginDTO.Password.Equals(cache.Password))
            {
                throw new InvalidOperationException("Password không hợp lệ.");
            }

            return await GenerateUserTokens(cache);
        }

        public Task Logout(string refreshToken)
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

        protected async Task<UserTokenDTO> GenerateUserTokens(UserCacheItem user)
        {
            UserDto userDTO = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Type = (CType)user.Type
            };

            var accessToken = await _securityService.GenerateAuthenToken(JsonConvert.SerializeObject(userDTO), 15);

            UserTokenDTO userToken = new UserTokenDTO
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Type = (CType)user.Type,
                AccessToken = accessToken,
            };

            return userToken;
        }
    }
}
