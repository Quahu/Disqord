namespace Disqord.Http;

/// <summary>
///     Represents when an HTTP response is returned relative to reading its content.
/// </summary>
public enum HttpCompletionMode
{
    /// <summary>
    ///     The response is returned after its content has been fully read.
    /// </summary>
    ContentRead,

    /// <summary>
    ///     The response is returned as soon as its headers have been read.
    /// </summary>
    HeadersRead,
}
