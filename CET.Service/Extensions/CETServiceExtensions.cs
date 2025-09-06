using CET.Service.Interface;
using CET.Service.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CET.Service.Extensions
{
    public static class CETServiceExtensions
    {
        public static IServiceCollection AddCommonMiddleware(this IServiceCollection services)
        {
            return services;
        }

        public static IApplicationBuilder UseCommonMiddleware(this IApplicationBuilder app)
        {
            {
                // app.UseMiniProfiler
            }

            return app;
        }

        public static IServiceCollection AddCETService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISecurityService, SecurityService>();
            return services;
        }
    }
}
