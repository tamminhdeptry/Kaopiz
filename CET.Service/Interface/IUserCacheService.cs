using CET.Domain.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET.Service.Interface
{
    public interface IUserCacheService
    {
        Task<UserCacheItem?> FindUserCacheByUserName(string userName);
    }
}
