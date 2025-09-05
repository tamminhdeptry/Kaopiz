using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET.Domain.Dtos
{
    public class ErrorDetailDto
    {
        public CErrorScope ErrorScope { get; set; }
        public string Field { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
    public enum CErrorScope
    {
        None = 0,
        Field = 1,
        FormSummary = 2,
        PageSumarry = 3,
        RedirectPage = 4,
        Global = 5,
        RedirectToLoginPage = 6
    }
}
