using CET.Domain.Enum;
using System.Data;

namespace CET.Domain.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public CType Type { get; set; }
    }

    public class UserTokenDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public CType Type { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }

    public class LoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
