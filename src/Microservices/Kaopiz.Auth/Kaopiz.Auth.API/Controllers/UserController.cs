using Microsoft.AspNetCore.Mvc;

namespace Kaopiz.Auth.API
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UserController : APIBaseController
    {
        public UserController(IHttpContextAccessor accessor) : base(accessor)
        {
        }
    }
}