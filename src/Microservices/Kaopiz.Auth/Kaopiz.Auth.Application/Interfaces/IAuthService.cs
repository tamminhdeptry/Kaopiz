using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Application
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task LogoutAsync();
    }
}
