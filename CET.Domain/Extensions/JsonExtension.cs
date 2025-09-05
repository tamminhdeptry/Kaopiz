using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET.Domain.Extensions
{
    public static class JsonExtension
    {
        public static string ToJson(this object objValue)
            => JsonConvert.SerializeObject(objValue) ?? string.Empty;

        public static T FromJson<T>(this string value) where T : new()
            => JsonConvert.DeserializeObject<T>(value: value) ?? new T();
    }
}
