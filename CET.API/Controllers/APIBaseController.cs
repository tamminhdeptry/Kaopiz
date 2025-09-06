using Microsoft.AspNetCore.Mvc;

namespace CET.API.Controllers
{
    public class APIBaseController
    {
        private readonly IHttpContextAccessor _contextAccessor;

        protected HttpContext _httpContext
        {
            get { return _contextAccessor.HttpContext; }
        }

        public APIBaseController(IHttpContextAccessor accessor)
        {
            _contextAccessor = accessor;
        }
    }
}
