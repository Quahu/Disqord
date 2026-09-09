using System;
using Disqord.Http;

namespace Disqord.Rest;

/// <summary>
///     Represents the failure of an attachment download.
/// </summary>
public class AttachmentDownloadException : Exception
{
    /// <summary>
    ///     Gets whether the failure was a non-success HTTP response,
    ///     in which case <see cref="StatusCode"/> is set.
    /// </summary>
    public bool IsHttpError => StatusCode != null;

    /// <summary>
    ///     Gets the HTTP status code of the failure response.
    ///     Returns <see langword="null"/> if the failure was not an HTTP response.
    /// </summary>
    public HttpResponseStatusCode? StatusCode { get; }

    /// <summary>
    ///     Gets the reason phrase of the failure response.
    ///     Returns <see langword="null"/> if the failure was not an HTTP response.
    /// </summary>
    public string? ReasonPhrase { get; }

    /// <summary>
    ///     Instantiates a new <see cref="AttachmentDownloadException"/> for a non-success HTTP response.
    /// </summary>
    /// <param name="statusCode"> The HTTP status code of the failure response. </param>
    /// <param name="reasonPhrase"> The reason phrase of the failure response. </param>
    public AttachmentDownloadException(HttpResponseStatusCode statusCode, string? reasonPhrase)
        : base(GetErrorMessage(statusCode, reasonPhrase))
    {
        StatusCode = statusCode;
        ReasonPhrase = reasonPhrase;
    }

    /// <summary>
    ///     Instantiates a new <see cref="AttachmentDownloadException"/> wrapping the exception that failed the download.
    /// </summary>
    /// <param name="innerException"> The exception that failed the download. </param>
    public AttachmentDownloadException(Exception innerException)
        : base("The attachment download failed. See the inner exception for details.", innerException)
    { }

    private static string GetErrorMessage(HttpResponseStatusCode statusCode, string? reasonPhrase)
    {
        var statusCodeText = Enum.IsDefined(statusCode)
            ? $"{(int) statusCode} {statusCode}"
            : ((int) statusCode).ToString();

        return $"HTTP: {statusCodeText}. Reason phrase: {reasonPhrase}";
    }
}
