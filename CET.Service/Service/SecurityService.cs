using CET.Domain.Caching;
using CET.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CET.Service.Service
{
    public class SecurityService : ISecurityService
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;
        public SecurityService(IConfiguration configuration, ICacheService cacheService)
        {
            _configuration = configuration;
            _cacheService = cacheService;
        }
        public string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public Task<string> DecryptJWTAccessToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return Task.FromResult(principal.Identity?.Name);
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }
        }

        public async Task<string> GenerateAuthenToken(string data, int expire = 15)
        {
            var id = Guid.NewGuid().ToString();
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, data),
            new Claim(JwtRegisteredClaimNames.Jti, id),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(expire),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                await _cacheService.SetCacheAsync(id, new RevokeTokenCacheItem { ExpiresAt = DateTime.UtcNow.AddMinutes(expire), Jti = id});
                return tokenHandler.WriteToken(token);
            }
        }

        public Task<string> GenerateRefreshToken(string data)
        {
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, data)
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(30),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Task.FromResult(tokenHandler.WriteToken(token));
            }
        }

        public Task<string> ValidateJwtAuthenToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return Task.FromResult(principal.Identity?.Name);
            }
            catch (SecurityTokenExpiredException)
            {
                return Task.FromResult<string?>(null);
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }
        }
    }
}
