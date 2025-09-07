using System.Diagnostics.CodeAnalysis;

namespace Kaopiz.Auth.Application
{
    public static class CollectionExtension
    {
        public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this ICollection<T>? collection)
        {
            return collection == null || !collection.Any();
        }
    }
}