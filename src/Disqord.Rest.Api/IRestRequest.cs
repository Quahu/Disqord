using System;
using System.Threading.Tasks;

namespace Disqord.Rest.Api
{
    /// <summary>
    ///     Represents a REST request to the Discord API.
    /// </summary>
    public interface IRestRequest : IDisposable
    {
        /// <summary>
        ///     Gets the route of this request.
        /// </summary>
        FormattedRoute Route { get; }

        /// <summary>
        ///     Gets the request content associated with this request.
        /// </summary>
        IRestRequestContent Content { get; }

        /// <summary>
        ///     Gets the request options of this request.
        /// </summary>
        IRestRequestOptions Options { get; }

        /// <summary>
        ///     Asynchronously waits for this request to be completed.
        /// </summary>
        /// <returns>
        ///     The REST response.
        /// </returns>
        Task<IRestResponse> WaitAsync();

        /// <summary>
        ///     Completes this request with a response.
        /// </summary>
        /// <param name="response"> The response. </param>
        void Complete(IRestResponse response);

        /// <summary>
        ///     Completes this request with an exception.
        /// </summary>
        /// <param name="exception"> The exception that occurred. </param>
        void Complete(Exception exception);
    }
}
