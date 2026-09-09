using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Http.Default;
using Qommon;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    /// <summary>
    ///     Downloads the attachment and returns the stream of its content.
    /// </summary>
    /// <remarks>
    ///     The content is downloaded as the returned stream is read,
    ///     similarly to <see cref="System.Net.Http.HttpClient.GetStreamAsync(string)"/>.
    /// </remarks>
    /// <param name="attachment"> The attachment to download. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the download with the result being the content stream.
    /// </returns>
    /// <exception cref="AttachmentDownloadException"> The download failed, e.g. due to a non-success HTTP response or an I/O error. </exception>
    public static Task<Stream> DownloadAsync(this IAttachment attachment, CancellationToken cancellationToken)
    {
        return attachment.DownloadAsync(false, cancellationToken);
    }

    /// <summary>
    ///     Downloads the attachment and returns the stream of its content.
    /// </summary>
    /// <remarks>
    ///     The content is downloaded as the returned stream is read,
    ///     similarly to <see cref="System.Net.Http.HttpClient.GetStreamAsync(string)"/>.
    /// </remarks>
    /// <param name="attachment"> The attachment to download. </param>
    /// <param name="useProxyUrl"> Whether to download from <see cref="IAttachment.ProxyUrl"/> instead of <see cref="IAttachment.Url"/>. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the download with the result being the content stream.
    /// </returns>
    /// <exception cref="AttachmentDownloadException"> The download failed, e.g. due to a non-success HTTP response or an I/O error. </exception>
    public static async Task<Stream> DownloadAsync(this IAttachment attachment, bool useProxyUrl = false, CancellationToken cancellationToken = default)
    {
        var client = attachment.GetRestClient();
        try
        {
            var response = await GetDownloadResponseAsync(client, attachment, useProxyUrl, cancellationToken).ConfigureAwait(false);
            return await OpenResponseStreamAsync(response, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (IsWrappable(ex, cancellationToken))
        {
            throw new AttachmentDownloadException(ex);
        }
    }

    /// <summary>
    ///     Downloads the attachment and copies its content to the specified stream.
    /// </summary>
    /// <param name="attachment"> The attachment to download. </param>
    /// <param name="destination"> The stream to copy the content to. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the download.
    /// </returns>
    /// <exception cref="AttachmentDownloadException"> The download failed, e.g. due to a non-success HTTP response or an I/O error. </exception>
    public static Task DownloadToAsync(this IAttachment attachment, Stream destination, CancellationToken cancellationToken)
    {
        return attachment.DownloadToAsync(destination, false, cancellationToken);
    }

    /// <summary>
    ///     Downloads the attachment and copies its content to the specified stream.
    /// </summary>
    /// <param name="attachment"> The attachment to download. </param>
    /// <param name="destination"> The stream to copy the content to. </param>
    /// <param name="useProxyUrl"> Whether to download from <see cref="IAttachment.ProxyUrl"/> instead of <see cref="IAttachment.Url"/>. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the download.
    /// </returns>
    /// <exception cref="AttachmentDownloadException"> The download failed, e.g. due to a non-success HTTP response or an I/O error. </exception>
    public static async Task DownloadToAsync(this IAttachment attachment, Stream destination, bool useProxyUrl = false, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(destination);
        Guard.CanWrite(destination);

        var client = attachment.GetRestClient();
        try
        {
            using var response = await GetDownloadResponseAsync(client, attachment, useProxyUrl, cancellationToken).ConfigureAwait(false);
            await CopyResponseToAsync(response, destination, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (IsWrappable(ex, cancellationToken))
        {
            throw new AttachmentDownloadException(ex);
        }
    }

    /// <summary>
    ///     Downloads the attachment and writes its content to the file at the specified path, creating or overwriting it.
    /// </summary>
    /// <param name="attachment"> The attachment to download. </param>
    /// <param name="path"> The path of the file to write the content to. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the download.
    /// </returns>
    /// <exception cref="AttachmentDownloadException"> The download failed, e.g. due to a non-success HTTP response or an I/O error. </exception>
    public static Task DownloadToFileAsync(this IAttachment attachment, string path, CancellationToken cancellationToken)
    {
        return attachment.DownloadToFileAsync(path, false, cancellationToken);
    }

    /// <summary>
    ///     Downloads the attachment and writes its content to the file at the specified path, creating or overwriting it.
    /// </summary>
    /// <param name="attachment"> The attachment to download. </param>
    /// <param name="path"> The path of the file to write the content to. </param>
    /// <param name="useProxyUrl"> Whether to download from <see cref="IAttachment.ProxyUrl"/> instead of <see cref="IAttachment.Url"/>. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the download.
    /// </returns>
    /// <exception cref="AttachmentDownloadException"> The download failed, e.g. due to a non-success HTTP response or an I/O error. </exception>
    public static async Task DownloadToFileAsync(this IAttachment attachment, string path, bool useProxyUrl = false, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(path);

        var client = attachment.GetRestClient();
        try
        {
            using var response = await GetDownloadResponseAsync(client, attachment, useProxyUrl, cancellationToken).ConfigureAwait(false);
            var fileStream = File.Create(path);
            try
            {
                await CopyResponseToAsync(response, fileStream, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                await fileStream.DisposeAsync().ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (IsWrappable(ex, cancellationToken))
        {
            throw new AttachmentDownloadException(ex);
        }
    }

    /// <summary>
    ///     Downloads the attachment and returns it as a <see cref="LocalAttachment"/> ready to be sent again.
    /// </summary>
    /// <param name="attachment"> The attachment to download. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the download with the result being the local attachment.
    /// </returns>
    /// <exception cref="AttachmentDownloadException"> The download failed, e.g. due to a non-success HTTP response or an I/O error. </exception>
    public static Task<LocalAttachment> DownloadAsLocalAttachmentAsync(this IAttachment attachment, CancellationToken cancellationToken)
    {
        return attachment.DownloadAsLocalAttachmentAsync(false, cancellationToken);
    }

    /// <summary>
    ///     Downloads the attachment and returns it as a <see cref="LocalAttachment"/> ready to be sent again.
    /// </summary>
    /// <param name="attachment"> The attachment to download. </param>
    /// <param name="useProxyUrl"> Whether to download from <see cref="IAttachment.ProxyUrl"/> instead of <see cref="IAttachment.Url"/>. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the download with the result being the local attachment.
    /// </returns>
    /// <exception cref="AttachmentDownloadException"> The download failed, e.g. due to a non-success HTTP response or an I/O error. </exception>
    public static async Task<LocalAttachment> DownloadAsLocalAttachmentAsync(this IAttachment attachment, bool useProxyUrl = false, CancellationToken cancellationToken = default)
    {
        var memoryStream = await attachment.DownloadToMemoryAsync(useProxyUrl, cancellationToken).ConfigureAwait(false);

        var fileName = attachment.FileName;
        var isSpoiler = attachment.Flags.HasFlag(AttachmentFlags.Spoiler);
        if (fileName.StartsWith(LocalAttachment.SpoilerPrefix, StringComparison.Ordinal))
        {
            fileName = fileName[LocalAttachment.SpoilerPrefix.Length..];
            isSpoiler = true;
        }

        var localAttachment = new LocalAttachment(memoryStream, fileName);

        var description = attachment.Description;
        if (!string.IsNullOrEmpty(description))
        {
            localAttachment.Description = description;
        }

        if (isSpoiler)
        {
            localAttachment.IsSpoiler = true;
        }

        return localAttachment;
    }

    /// <summary>
    ///     Downloads the attachment and returns its content as an in-memory stream positioned at its beginning.
    /// </summary>
    /// <param name="attachment"> The attachment to download. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the download with the result being the in-memory stream.
    /// </returns>
    /// <exception cref="AttachmentDownloadException"> The download failed, e.g. due to a non-success HTTP response or an I/O error. </exception>
    public static Task<MemoryStream> DownloadToMemoryAsync(this IAttachment attachment, CancellationToken cancellationToken)
    {
        return attachment.DownloadToMemoryAsync(false, cancellationToken);
    }

    /// <summary>
    ///     Downloads the attachment and returns its content as an in-memory stream positioned at its beginning.
    /// </summary>
    /// <param name="attachment"> The attachment to download. </param>
    /// <param name="useProxyUrl"> Whether to download from <see cref="IAttachment.ProxyUrl"/> instead of <see cref="IAttachment.Url"/>. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the download with the result being the in-memory stream.
    /// </returns>
    /// <exception cref="AttachmentDownloadException"> The download failed, e.g. due to a non-success HTTP response or an I/O error. </exception>
    public static async Task<MemoryStream> DownloadToMemoryAsync(this IAttachment attachment, bool useProxyUrl = false, CancellationToken cancellationToken = default)
    {
        var client = attachment.GetRestClient();
        try
        {
            using var response = await GetDownloadResponseAsync(client, attachment, useProxyUrl, cancellationToken).ConfigureAwait(false);
            var memoryStream = new MemoryStream(attachment.FileSize);
            await CopyResponseToAsync(response, memoryStream, cancellationToken).ConfigureAwait(false);
            memoryStream.Position = 0;
            return memoryStream;
        }
        catch (Exception ex) when (IsWrappable(ex, cancellationToken))
        {
            throw new AttachmentDownloadException(ex);
        }
    }

    private static async Task<IHttpResponse> GetDownloadResponseAsync(IRestClient client, IAttachment attachment, bool useProxyUrl, CancellationToken cancellationToken)
    {
        var url = useProxyUrl
            ? attachment.ProxyUrl
            : attachment.Url;

        using var request = new DefaultHttpRequest(HttpRequestMethod.Get, new Uri(url), null, HttpCompletionMode.HeadersRead);
        var response = await client.ApiClient.Requester.ExecuteAuxiliaryAsync(request, cancellationToken).ConfigureAwait(false);

        var statusCode = response.StatusCode;
        if (statusCode.IsSuccess())
        {
            return response;
        }

        var reasonPhrase = response.ReasonPhrase;
        response.Dispose();
        throw new AttachmentDownloadException(statusCode, reasonPhrase);
    }

    private static async Task<Stream> OpenResponseStreamAsync(IHttpResponse response, CancellationToken cancellationToken)
    {
        try
        {
            var stream = await response.ReadAsync(cancellationToken).ConfigureAwait(false);
            return new HttpResponseStream(response, stream);
        }
        catch
        {
            response.Dispose();
            throw;
        }
    }

    private static async Task CopyResponseToAsync(IHttpResponse response, Stream destination, CancellationToken cancellationToken)
    {
        var stream = await response.ReadAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await stream.CopyToAsync(destination, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            await stream.DisposeAsync().ConfigureAwait(false);
        }
    }

    private static bool IsWrappable(Exception exception, CancellationToken cancellationToken)
    {
        if (exception is AttachmentDownloadException)
        {
            return false;
        }

        var isCallerCancellation = exception is OperationCanceledException && cancellationToken.IsCancellationRequested;
        return !isCallerCancellation;
    }
}
