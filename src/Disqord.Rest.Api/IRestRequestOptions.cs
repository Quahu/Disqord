using System.Collections.Generic;
using System.Threading;

namespace Disqord.Rest.Api
{
    /// <summary>
    ///     Represents a set of options for a REST request.
    /// </summary>
    public interface IRestRequestOptions
    {
        /// <summary>
        ///     Gets or sets the cancellation token for the request.
        /// </summary>
        CancellationToken CancellationToken { get; set; }

        /// <summary>
        ///     Gets or sets the headers for the request.
        /// </summary>
        IDictionary<string, string> Headers { get; set; }
    }
}