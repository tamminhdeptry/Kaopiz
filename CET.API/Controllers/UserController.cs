using Microsoft.AspNetCore.Mvc;
using CET.Service.Interface;

namespace CET.API.Controllers
{
    [ApiVersion("1")]
    public class UserController : APIBaseController
    {
        private readonly IUserService _userService;
        public UserController(IHttpContextAccessor accessor,
            IUserService userService ) : base(accessor)
        {
            _userService = userService;
        }

    }
}
