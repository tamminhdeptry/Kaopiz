using Kaopiz.Auth.Application;
using Kaopiz.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kaopiz.Auth.API
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UserController : APIBaseController
    {
        private readonly IUserService _userService;
        public UserController(IHttpContextAccessor accessor, IUserService userService) : base(accessor)
        {
            _userService = userService;
        }

        [HttpGet("myprofile")]
        [Authorize]
        public async Task<ApiResponse<UserDto>> GetMyProfile()
        {
            return await _userService.GetMyProfileAsync();
        }
    }
}