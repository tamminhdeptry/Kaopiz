using Microsoft.AspNetCore.Mvc;

namespace Kaopiz.Auth.API
{
    [ApiController]
    public class APIBaseController
    {
        private readonly IHttpContextAccessor _contextAccessor;

        protected HttpContext? _httpContext
        {
            get { return _contextAccessor.HttpContext; }
        }

        public APIBaseController(IHttpContextAccessor accessor)
        {
            _contextAccessor = accessor;
        }
    }
}
