using Kaopiz.Auth.Domain;
using System.Net;
using Kaopiz.Auth.Infrastructure;
using Microsoft.Extensions.Logging;
using Kaopiz.Shared.Contracts;


namespace Kaopiz.Auth.Application
{
    public class AuthService : IAuthService
    {
        private readonly ISecurityService _securityService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserCacheService _userCacheService;
        private readonly ICacheService _cacheService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            ISecurityService securityService,
            IJwtTokenService jwtTokenService,
            IUserCacheService userCacheService,
            IUserRepository userRepository,
            ICacheService cacheService,
            ILogger<AuthService> logger)
        {
            _securityService = securityService;
            _userCacheService = userCacheService;
            _userRepository = userRepository;
            _cacheService = cacheService;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        /// <summary>
        /// Handle user login with User and Password
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="KaopizException"></exception>
        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var modelStateValidateResult = loginRequestDto.ModelStateValidate();
            if (!modelStateValidateResult.GetErrors().IsNullOrEmpty())
            {
                throw new KaopizException(errors: modelStateValidateResult.GetErrors(), statusCode: HttpStatusCode.BadRequest);
            }

            var response = new ApiResponse<LoginResponseDto>()
            {
                StatusCode = HttpStatusCode.OK,
            };

            var errorDto = new ErrorDetailDto();

            var userCacheItem = await _userCacheService.FindUserCacheByUserName(loginRequestDto.UserName);

            if (userCacheItem == null || userCacheItem.Type != loginRequestDto.Type)
            {
                errorDto.Error = $"UserName = '{loginRequestDto.UserName}' not exist.";
                errorDto.ErrorScope = CErrorScope.Field;
                errorDto.Field = nameof(loginRequestDto.UserName);
                throw new KaopizException(errorDto: errorDto);
            }

            loginRequestDto.Password = _securityService.ComputeSha256Hash(loginRequestDto.Password);

            if (!loginRequestDto.Password.Equals(userCacheItem.Password))
            {
                errorDto.Error = $"Password is invalid.";
                errorDto.ErrorScope = CErrorScope.Field;
                errorDto.Field = nameof(loginRequestDto.Password);
                throw new KaopizException(errorDto: errorDto);
            }

            LoginResponseDto loginResponseDto = await GenerateUserTokens(userCacheItem, loginRequestDto.RememberMe);
            response.Result.Data = loginResponseDto;
            response.Result.Success = true;
            return response;
        }

        /// <summary>
        /// Handle user registration.
        /// </summary>
        /// <param name="registerRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="KaopizException"></exception>
        public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var modelStateValidateResult = registerRequestDto.ModelStateValidate();
            if (!modelStateValidateResult.GetErrors().IsNullOrEmpty())
            {
                throw new KaopizException(errors: modelStateValidateResult.GetErrors(), statusCode: HttpStatusCode.BadRequest);
            }

            var response = new ApiResponse<RegisterResponseDto>()
            {
                StatusCode = HttpStatusCode.OK,
            };

            var errorDto = new ErrorDetailDto();

            var userCacheItem = await _userCacheService.FindUserCacheByUserName(registerRequestDto.UserName);

            if (userCacheItem != null)
            {
                errorDto.Error = $"UserName = '{registerRequestDto.UserName}' already exist.";
                errorDto.ErrorScope = CErrorScope.Field;
                errorDto.Field = nameof(RegisterRequestDto.UserName);
                throw new KaopizException(errorDto: errorDto, statusCode: HttpStatusCode.BadRequest);
            }

            var userEntity = new UserEntity()
            {
                Password = _securityService.ComputeSha256Hash(registerRequestDto.Password),
                UserName = registerRequestDto.UserName,
                DisplayName = registerRequestDto.DisplayName,
                Type = registerRequestDto.Type
            };

            await _userRepository.AddAsync(userEntity: userEntity);

            await _userCacheService.SetUserCacheAsync(new UserCacheItem()
            {
                Id = userEntity.Id,
                DisplayName = userEntity.DisplayName,
                Password = userEntity.Password,
                Type = userEntity.Type,
                UserName = userEntity.UserName
            });

            response.Result.Data = new RegisterResponseDto()
            {
                Message = "Congratulation! Registration successfully."
            };

            return response;
        }
        
        public async Task LogoutAsync()
        {
            string jti = RuntimeContext.CurrentJti;
            var revokedTokenCacheItem = await _cacheService.GetCacheAsync<RevokeTokenCacheItem>(cacheKey: jti);
            if (revokedTokenCacheItem != null)
            {
                revokedTokenCacheItem.RevokedAt = DateTimeOffset.UtcNow;
                await _cacheService.SetCacheAsync<RevokeTokenCacheItem>(cacheKey: jti,
                    value: revokedTokenCacheItem,
                    ttl: revokedTokenCacheItem.ExpiresAt - DateTimeOffset.UtcNow);
            }
        }

        private async Task<LoginResponseDto> GenerateUserTokens(UserCacheItem user, bool rememberMe = false)
        {
            UserDto userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Type = (CUserType)user.Type
            };
            // If remember me is true -> token expired after 30 days, if not expired after 1 day
            string accessToken = await _jwtTokenService.GenerateAuthenToken(userDto, rememberMe ? 30 * 24 * 60 : 24 * 60);

            return new LoginResponseDto()
            {
                AccessToken = accessToken
            };
        }
    }
}
