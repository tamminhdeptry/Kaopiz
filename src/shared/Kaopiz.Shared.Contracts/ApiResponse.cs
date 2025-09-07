using System.Net;
namespace Kaopiz.Shared.Contracts
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public ResponseResult<T> Result { get; set; } = new ResponseResult<T>();
    }

    public class ResponseResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public List<ErrorDetailDto> Errors { get; set; } = new List<ErrorDetailDto>();
    }
}
