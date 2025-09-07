using Kaopiz.Auth.Infrastructure;
using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Application
{
    public class UserService : IUserService
    {
        public Task<UserEntity> GetUserById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserEntity> GetUserByLoginRequestDto(LoginRequestDto loginRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
