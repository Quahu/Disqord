using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Logging;
using Disqord.Utilities.Binding;

namespace Disqord.Rest.Api
{
    /// <summary>
    ///     Represents the type responsible for sending REST requests and completing them with HTTP response data.
    /// </summary>
    public interface IRestRequester : IBindable<IRestApiClient>, ILogging, IDisposable
    {
        /// <summary>
        ///     Gets the REST API client of this requester.
        /// </summary>
        IRestApiClient ApiClient { get; }

        /// <summary>
        ///     Gets the Discord API version of this requester.
        /// </summary>
        int Version { get; }

        /// <summary>
        ///     Gets the HTTP client of this requester.
        /// </summary>
        IHttpClient HttpClient { get; }

        /// <summary>
        ///     Executes the specified REST request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteAsync(IRestRequest request, CancellationToken cancellationToken = default);
    }
}
