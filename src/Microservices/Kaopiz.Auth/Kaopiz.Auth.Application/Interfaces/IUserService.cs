using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Application
{
    public interface IUserService
    {
        Task<ApiResponse<UserDto>> GetMyProfileAsync();
    }
}
