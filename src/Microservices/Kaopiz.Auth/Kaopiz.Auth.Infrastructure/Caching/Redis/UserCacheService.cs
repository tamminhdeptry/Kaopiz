using System.Net;
using Kaopiz.Auth.Domain;
using Kaopiz.Auth.Application;
using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Infrastructure
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

        public async Task<bool> CheckUserCacheExistByUserName(string userName)
        {
            return await _cacheService.GetCacheAsync<UserCacheItem>(cacheKey: userName) != null;
        }

        public async Task<UserCacheItem?> FindUserCacheByUserName(string userName)
        {
            var userCacheItem = await _cacheService.GetCacheAsync<UserCacheItem>(userName);
            if (userCacheItem == null)
            {
                var user = await _userRepository.GetUserByUserName(userName);
                if (user == null) return null;
                userCacheItem = new UserCacheItem
                {
                    Id = user.Id,
                    Password = user.Password,
                    UserName = user.UserName,
                    DisplayName = user.DisplayName,
                    Type = user.Type
                };
                await _cacheService.SetCacheAsync<UserCacheItem>(user.UserName, userCacheItem, ttl: TimeSpan.FromMinutes(15));
            }

            return userCacheItem;
        }

        public async Task SetUserCacheAsync(UserCacheItem userCacheItem)
        {
            if (userCacheItem == null)
            {
                throw new KaopizException(new ErrorDetailDto()
                {
                    Error = $"Cache data is null or cache key is null.",
                    ErrorScope = CErrorScope.PageSumarry,
                }, HttpStatusCode.BadRequest);
            }

            await _cacheService.SetCacheAsync<UserCacheItem>(cacheKey: userCacheItem.UserName, value: userCacheItem,
                ttl: TimeSpan.FromMinutes(15));
        }
    }
}
