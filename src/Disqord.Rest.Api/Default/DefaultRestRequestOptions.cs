using System;
using System.Collections.Generic;
using Disqord.Rest.Api;
using Disqord.Rest.Api.Default;

namespace Disqord.Rest
{
    /// <summary>
    ///     Represents the default set of <see cref="IRestRequestOptions"/>.
    /// </summary>
    public class DefaultRestRequestOptions : IRestRequestOptions
    {
        /// <inheritdoc/>
        public string Reason
        {
            get
            {
                var headers = _headers;
                if (headers != null && headers.TryGetValue("X-Audit-Log-Reason", out var value))
                    return value;

                return null;
            }
            set => Headers["X-Audit-Log-Reason"] = value;
        }

        /// <summary>
        ///     Gets or sets the custom headers for the request.
        /// </summary>
        public IDictionary<string, string> Headers
        {
            get => _headers ??= new Dictionary<string, string>();
            set => _headers = value;
        }
        private IDictionary<string, string> _headers;

        /// <summary>
        ///     Gets or sets the action that should be performed
        ///     on the <see cref="IRestRequest"/> before sending it.
        /// </summary>
        public Action<IRestRequest> RequestAction { get; set; }

        /// <summary>
        ///     Gets or sets the action that should be performed
        ///     after the REST request is executed.
        /// </summary>
        /// <example>
        ///     Dumping the rate-limit information for a REST method call.
        ///     <code language="csharp">
        ///     await channel.SendMessageAsync(message, options: new DefaultRestRequestOptions
        ///     {
        ///         HeadersAction = headers => Logger.LogInformation("{0} of {1} requests remain.", headers.Remaining, headers.Limit)
        ///     });
        ///     </code>
        /// </example>
        public Action<DefaultRestResponseHeaders> HeadersAction { get; set; }
    }
}
