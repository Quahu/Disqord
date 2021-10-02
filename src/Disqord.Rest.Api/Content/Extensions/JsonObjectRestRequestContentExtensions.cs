using System.Runtime.CompilerServices;

namespace Disqord.Rest.Api
{
    internal static class JsonObjectRestRequestContentExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static JsonObjectRestRequestContent<T[]> ToContent<T>(this T[] contents)
            => new(contents);
    }
}
