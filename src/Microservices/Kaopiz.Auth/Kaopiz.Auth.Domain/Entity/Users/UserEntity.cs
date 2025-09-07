
using Kaopiz.Auth.Domain;
using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Infrastructure
{
    public class UserEntity : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public CUserType Type { get; set; }
    }
}
