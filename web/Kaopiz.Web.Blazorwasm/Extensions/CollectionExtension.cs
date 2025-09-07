using System.Diagnostics.CodeAnalysis;

namespace Kaopiz.Web.Blazorwasm
{
    public static class CollectionExtension
    {
        public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this ICollection<T>? collection)
        {
            return collection == null || !collection.Any();
        }
    }
}