using CET.Domain.Caching;
using CET.Domain.Enum;
using CET.Service.Interface;

namespace CET.Infrastructure.Caching.Redis
{
    public class UserCacheService : IUserCacheService
    {
        private readonly ICacheService _cacheService;
        private readonly IUserRepository _userRepository;
        public UserCacheService(ICacheService cacheService, IUserRepository userRepository)
        {
            _cacheService = cacheService;
            _userRepository = userRepository;
        }

        public async Task<UserCacheItem?> FindUserCacheByUserName(string userName)
        {
            var userCacheItem = await _cacheService.GetCacheAsync<UserCacheItem>(userName);
            if (userCacheItem == null)
            {
                var user = await _userRepository.GetUserByUserName(userName);
                if (user == null) return null;
                userCacheItem = new UserCacheItem { Id = user.Id, Password = user.Password, UserName = user.UserName,
                    DisplayName = user.DisplayName,
                    Type = (CType)user.Type
                };
                await _cacheService.SetCacheAsync<UserCacheItem>(user.UserName, userCacheItem, ttl: TimeSpan.FromMinutes(15));
            }

            return userCacheItem;
        }
    }
}
