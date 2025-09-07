using System.ComponentModel.DataAnnotations;

namespace Kaopiz.Shared.Contracts
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public CUserType Type { get; set; }
    }

    public class UserTokenDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public CUserType Type { get; set; }
        public string? AccessToken { get; set; }
    }
}
