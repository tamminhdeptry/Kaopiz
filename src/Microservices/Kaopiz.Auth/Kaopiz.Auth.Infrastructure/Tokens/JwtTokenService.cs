using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Kaopiz.Auth.Application;
using Kaopiz.Auth.Domain;
using Kaopiz.Shared.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Kaopiz.Auth.Infrastructure
{
    internal class JwtTokenService : IJwtTokenService
    {
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;

        public JwtTokenService(
            ICacheService cacheService,
            IConfiguration configuration
        )
        {
            _cacheService = cacheService;
            _configuration = configuration;
        }
        public async Task<string> GenerateAuthenToken(UserDto userDto, int expire = 15)
        {
            var id = Guid.NewGuid().ToString();
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                    new Claim(ClaimTypes.Role, userDto.Type.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, id),
                    new Claim(JwtRegisteredClaimNames.Iat,
                        DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                        ClaimValueTypes.Integer64)
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(expire),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s: _configuration["Jwt:Key"] ?? string.Empty)),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var expiresAt = DateTime.UtcNow.AddMinutes(expire);

                await _cacheService.SetCacheAsync(
                    id,
                    new RevokeTokenCacheItem
                    {
                        ExpiresAt = expiresAt,
                        Jti = id
                    },
                    ttl: expiresAt - DateTime.UtcNow
                );
                return tokenHandler.WriteToken(token);
            }
        }
    }
}