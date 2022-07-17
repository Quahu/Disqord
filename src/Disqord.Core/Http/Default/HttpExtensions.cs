using System.Net.Http.Headers;

namespace Disqord.Http.Default;

/// <summary>
///     Defines utility methods for the System.Net.Http namespace.
/// </summary>
public static class HttpExtensions
{
    /// <summary>
    ///     Attempts to remove and then add the specified header and its value into the <see cref="HttpHeaders"/> collection.
    /// </summary>
    /// <param name="headers"> The headers to modify. </param>
    /// <param name="name"> The name of the header to set in the collection. </param>
    /// <param name="value"> The content of the header. </param>
    public static void Set(this HttpHeaders headers, string name, string value)
    {
        headers.Remove(name);
        headers.Add(name, value);
    }
}
