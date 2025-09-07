using Kaopiz.Auth.Domain;

namespace Kaopiz.Auth.Application
{
    public interface IUserCacheService
    {
        Task<bool> CheckUserCacheExistByUserName(string userName);
        Task<UserCacheItem?> FindUserCacheByUserName(string userName);
        Task SetUserCacheAsync(UserCacheItem userCacheItem);
    }
}
