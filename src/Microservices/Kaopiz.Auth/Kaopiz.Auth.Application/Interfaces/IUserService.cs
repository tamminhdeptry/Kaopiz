using Kaopiz.Auth.Infrastructure;
using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Application
{
    public interface IUserService
    {
        Task<UserEntity> GetUserById(Guid userId);
        Task<UserEntity> GetUserByLoginRequestDto(LoginRequestDto loginRequestDto);
    }
}
