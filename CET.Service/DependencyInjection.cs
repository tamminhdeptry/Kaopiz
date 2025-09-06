using CET.Domain;
using CET.Domain.Caching;
using CET.Domain.Dtos;
using CET.Domain.Extensions;
using CET.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace CET.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddKaopizService(this IServiceCollection services, IConfiguration configuration)
        {
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
                            string? jti = context.Principal.FindFirstValue(JwtRegisteredClaimNames.Jti);
                            if (jti != string.Empty)
                            {
                                var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();
                                if (cacheService != null)
                                {
                                    var cache = await cacheService.GetCacheAsync<RevokeTokenCacheItem>(jti);
                                    if (cache != null && cache.RevokedAt.HasValue && cache.RevokedAt.Value < DateTimeOffset.UtcNow)
                                    {
                                        context.Fail("token da bi revoke");
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
            return services;
        }
    }
}
