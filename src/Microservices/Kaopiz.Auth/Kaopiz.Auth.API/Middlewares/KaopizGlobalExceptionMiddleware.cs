using System.Net;
using Kaopiz.Auth.Domain;
using Kaopiz.Shared.Contracts;
using Newtonsoft.Json;

namespace Kaopiz.Auth.API
{
    public class KaopizGlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<KaopizGlobalExceptionMiddleware> _logger;

        public KaopizGlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<KaopizGlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var response = new ApiResponse<object>();
            var result = new ResponseResult<object>();

            try
            {
                await _next(context: httpContext);
            }
            catch (KaopizException ex)
            {
                _logger.LogWarning(message: $"{string.Join(", ", ex.Errors)}");
                response.StatusCode = ex.StatusCode;
                response.Result.Success = false;
                response.Result.Errors.AddRange(ex.Errors);

                string payload = JsonConvert.SerializeObject(response);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)response.StatusCode;

                await httpContext.Response.WriteAsync(payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(message: $"Error = {ex.Message}, StackTrace = {ex.StackTrace}");
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Result.Success = false;
                response.Result.Errors.Add(item: new ErrorDetailDto()
                {
                    Error = $"Internal server error. ErrorMessage = {ex.Message}",
                    ErrorScope = CErrorScope.Global,
                });

                string payload = JsonConvert.SerializeObject(response);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)response.StatusCode;

                await httpContext.Response.WriteAsync(payload);
            }
        }
    }
}