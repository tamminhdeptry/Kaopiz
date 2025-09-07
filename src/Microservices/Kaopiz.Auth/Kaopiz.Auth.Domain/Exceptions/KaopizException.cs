using System.Net;
using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Domain
{
    public class KaopizException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<ErrorDetailDto> Errors { get; set; } = new List<ErrorDetailDto>();

        public KaopizException(ErrorDetailDto errorDto, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : base(message: errorDto.Error)
        {
            StatusCode = statusCode;
            Errors.Add(item: errorDto);
        }

        public KaopizException(IEnumerable<ErrorDetailDto> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : base(string.Join(", ", errors.Select(s => s.Error)))
        {
            StatusCode = statusCode;
            Errors.AddRange(errors);
        }
    }
}