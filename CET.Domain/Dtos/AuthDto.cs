using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET.Domain.Dtos
{
    public class AuthDto
    {
    }

    public class RefreshTokenDTO
    {
        public string RefreshToken { get; set; }
    }

    public class TokenResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
