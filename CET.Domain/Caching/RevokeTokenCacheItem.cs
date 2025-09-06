using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET.Domain.Caching
{
    public class RevokeTokenCacheItem
    {
        public string Jti { get; set; } = string.Empty;
        public DateTimeOffset? RevokedAt { get; set; } = null;
        public DateTimeOffset? ExpiresAt { get; set; }
    }
}
