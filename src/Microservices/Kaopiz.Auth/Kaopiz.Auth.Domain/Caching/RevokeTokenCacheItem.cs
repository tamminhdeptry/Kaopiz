namespace Kaopiz.Auth.Domain
{
    public class RevokeTokenCacheItem
    {
        public string Jti { get; set; } = string.Empty;
        public DateTimeOffset? RevokedAt { get; set; } = null;
        public DateTimeOffset? ExpiresAt { get; set; }
    }
}
