using System;
using System.Collections.Generic;
using System.Threading;
using Disqord.Rest.Api;
using Disqord.Rest.Api.Default;

namespace Disqord.Rest.Default
{
    public class DefaultRestRequestOptions : IRestRequestOptions
    {
        /// <summary>
        ///     Gets or sets the cancellation token for the request.
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        ///     Gets or sets the audit log reason for the request.
        ///     This is a shortcut for setting the <c>X-Audit-Log-Reason</c> header in <see cref="Headers"/>.
        /// </summary>
        public string Reason
        {
            get
            {
                if (_headers != null && _headers.TryGetValue("X-Audit-Log-Reason", out var value))
                    return value;

                return null;
            }
            set
            {
                Headers["X-Audit-Log-Reason"] = value;
            }
        }

        /// <summary>
        ///     Gets or sets the custom headers for the request.
        /// </summary>
        public IDictionary<string, string> Headers
        {
            get
            {
                if (_headers == null)
                    _headers = new Dictionary<string, string>();

                return _headers;
            }
            set
            {
                _headers = value;
            }
        }
        private IDictionary<string, string> _headers;

        public Action<IRestRequest> RequestAction { get; set; }

        /// <summary>
        ///     Gets or sets whether to bypass the pre-emptive
        ///     rate-limit delay in the <see cref="DefaultRestRateLimiter"/>.
        ///     Defaults to <see langword="false"/>.
        ///     Setting this to <see langword="true"/> will lead to <c>429</c>s, if not used with caution.
        /// </summary>
        /// <remarks>
        ///     This should only be set for broken Discord endpoints, such as
        ///     modifying a channel or modifying the current user, as those
        ///     have multiple separate rate-limits which currently cannot be
        ///     accounted for from the library's standpoint.
        /// </remarks>
        public bool BypassRateLimitDelay { get; set; }
    }
}
