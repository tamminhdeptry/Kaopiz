using System.Net;
using Kaopiz.Auth.Domain;
using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(
            IUserRepository userRepository
        )
        {
            _userRepository = userRepository;
        }
        public async Task<ApiResponse<UserDto>> GetMyProfileAsync()
        {
            if (RuntimeContext.CurrentUserId == Guid.Empty)
            {
                throw new KaopizException(errorDto: new ErrorDetailDto()
                {
                    Error = $"Unauthorized",
                    ErrorScope = CErrorScope.PageSumarry
                }, statusCode: HttpStatusCode.Unauthorized);
            }

            var user = await _userRepository.GetUserByUserId(RuntimeContext.CurrentUserId);

            if (user == null)
            {
                throw new KaopizException(errorDto: new ErrorDetailDto()
                {
                    Error = $"User with ID = {RuntimeContext.CurrentUserId} not found",
                    ErrorScope = CErrorScope.PageSumarry
                }, statusCode: HttpStatusCode.NotFound);
            }

            return new ApiResponse<UserDto>()
            {
                Result = new ResponseResult<UserDto>()
                {
                    Data = new UserDto()
                    {
                        DisplayName = user.DisplayName,
                        Id = user.Id,
                        Type = user.Type,
                        UserName = user.UserName
                    },
                    Success = true
                },
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
