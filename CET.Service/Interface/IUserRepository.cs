using CET.Infrastructure.Entity.Users;

namespace CET.Service.Interface
{
    public interface IUserRepository
    {
        Task<UserEntity> GetUserByUserName(string userName);
        Task<UserEntity> GetUserByUserId(Guid id);
        Task<UserEntity> AddAsync(UserEntity userEntity);
    }
}
