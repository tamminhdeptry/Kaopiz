using CET.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET.Domain.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public CType Type { get; set; }
    }
}
