using CET.Domain.Dtos;
using CET.Infrastructure.Entity.Users;
using CET.Service.Interface;

namespace CET.Service.Service
{
    public class UserService : IUserService
    {
        public Task<UserEntity> GetUserById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserEntity> GetUserByLoginDTO(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }
    }
}
