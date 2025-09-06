using CET.Domain.Dtos;
using CET.Infrastructure.Entity.Users;

namespace CET.Service.Interface
{
    public interface IUserService
    {
        Task<UserEntity> GetUserById(Guid userId);
        Task<UserEntity> GetUserByLoginDTO(LoginDTO loginDTO);
    }
}
