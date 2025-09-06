using CET.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET.Domain.Caching
{
    public class UserCacheItem
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Password {  get; set; }
        public CType Type { get; set; }
    }
}
