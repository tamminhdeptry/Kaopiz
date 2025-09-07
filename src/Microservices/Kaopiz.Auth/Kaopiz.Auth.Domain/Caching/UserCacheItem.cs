using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Domain
{
    public class UserCacheItem
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
        public CUserType Type { get; set; }
    }
}
