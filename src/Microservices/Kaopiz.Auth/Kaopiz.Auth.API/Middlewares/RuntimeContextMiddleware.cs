using Kaopiz.Auth.Application;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Kaopiz.Auth.API
{
    public class RuntimeContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RuntimeContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                string? userIdClaim = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdClaim, out var userId))
                {
                    RuntimeContext.CurrentUserId = userId;
                }
                else
                {
                    RuntimeContext.CurrentUserId = Guid.Empty;
                }

                string? jti = context.User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                RuntimeContext.CurrentJti = jti ?? string.Empty;

                string? ip = context.Connection.RemoteIpAddress?.ToString();
                RuntimeContext.CurrentIPAddress = ip ?? string.Empty;

                string? token = context.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
                {
                    RuntimeContext.CurrentAccessToken = token.Substring("Bearer ".Length);
                }

                await _next(context);
            }
            finally
            {
                RuntimeContext.CurrentUserId = Guid.Empty;
                RuntimeContext.CurrentIPAddress = string.Empty;
                RuntimeContext.CurrentAccessToken = string.Empty;
            }
        }
    }
}