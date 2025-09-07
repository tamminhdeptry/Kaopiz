using Kaopiz.Auth.Infrastructure;

namespace Kaopiz.Auth.Application
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetUserByUserId(Guid id, bool disableTracking = false);
        Task<UserEntity?> GetUserByUserName(string userName, bool disableTracking = false);
        Task<UserEntity> AddAsync(UserEntity userEntity, bool needSaveChange = true,
            CancellationToken cancellationToken = default);
    }
}
