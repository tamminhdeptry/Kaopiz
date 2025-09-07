using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Application
{
    public interface IJwtTokenService
    {
        Task<string> GenerateAuthenToken(UserDto userDto, int expire = 15);
    }
}