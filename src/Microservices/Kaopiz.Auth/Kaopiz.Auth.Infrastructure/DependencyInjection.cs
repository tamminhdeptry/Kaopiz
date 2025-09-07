using Kaopiz.Auth.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Kaopiz.Auth.Domain;
using Microsoft.AspNetCore.Http;
using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["CachingConfig:Redis:ConnectionString"];
                options.InstanceName = configuration["CachingConfig:Redis:InstanceName"];
            });

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(config =>
                {
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        RequireAudience = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(s: configuration["Jwt:Key"] ?? string.Empty))
                    };
                    config.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = async context =>
                        {
                            string? jti = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);
                            if (!string.IsNullOrEmpty(jti))
                            {
                                var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();
                                if (cacheService != null)
                                {
                                    var cache = await cacheService.GetCacheAsync<RevokeTokenCacheItem>(jti);
                                    if (cache != null && cache.RevokedAt.HasValue && cache.RevokedAt.Value < DateTimeOffset.UtcNow)
                                    {
                                        context.Fail("Token has been revoked.");
                                        return;
                                    }
                                }
                            }
                        },
                        OnAuthenticationFailed = async auth =>
                        {
                            await Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            string errorMessage = "Unauthorized";
                            if (context.HttpContext.Items.TryGetValue("AuthException", out var exceptionObj) &&
                                exceptionObj is Exception ex)
                            {
                                errorMessage = ex.Message;
                            }

                            var responseResult = new ResponseResult<string>()
                            {
                                Data = null,
                                Errors = new List<ErrorDetailDto>()
                                {
                                    new ErrorDetailDto()
                                    {
                                        Error = errorMessage,
                                        ErrorScope = CErrorScope.PageSumarry
                                    }
                                },
                                Success = false
                            };

                            return context.Response.WriteAsync(responseResult.ToJson());
                        }
                    };
                });

            services.AddAuthorization();

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IUserCacheService, UserCacheService>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            return services;
        }
    }
}
