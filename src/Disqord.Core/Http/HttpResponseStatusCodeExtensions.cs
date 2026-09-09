namespace Disqord.Http;

/// <summary>
///     Defines extension methods for <see cref="HttpResponseStatusCode"/>.
/// </summary>
public static class HttpResponseStatusCodeExtensions
{
    /// <summary>
    ///     Gets whether the status code is in the <c>2xx</c> range.
    /// </summary>
    /// <param name="statusCode"> The status code to check. </param>
    /// <returns>
    ///     <see langword="true"/> if the status code indicates success.
    /// </returns>
    public static bool IsSuccess(this HttpResponseStatusCode statusCode)
    {
        var code = (int) statusCode;
        return code > 199 && code < 300;
    }

    /// <summary>
    ///     Gets whether the status code is in the <c>5xx</c> range.
    /// </summary>
    /// <param name="statusCode"> The status code to check. </param>
    /// <returns>
    ///     <see langword="true"/> if the status code indicates a server error.
    /// </returns>
    public static bool IsServerError(this HttpResponseStatusCode statusCode)
    {
        var code = (int) statusCode;
        return code > 499 && code < 600;
    }
}
