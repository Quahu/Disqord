using System.Runtime.CompilerServices;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

internal static class JsonObjectRestRequestContentExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static JsonObjectRestRequestContent<T[]> ToObjectContent<T>(this T[] models)
        where T : JsonModel
    {
        return new(models);
    }
}
