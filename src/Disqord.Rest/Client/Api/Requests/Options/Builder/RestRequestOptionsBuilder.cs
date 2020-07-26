using System;
using System.Threading;

namespace Disqord.Rest
{
    public sealed class RestRequestOptionsBuilder
    {
        /// <summary>
        ///     Gets or sets the <see cref="TimeSpan"/> representing how long the REST client should wait before timing out the request.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="System.Threading.CancellationToken"/> representing the cancellation token for the REST request.
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        ///     Gets or sets the audit log reason.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        ///     Gets or sets the maximum rate-limit duration to delay for instead of throwing.
        /// </summary>
        public TimeSpan MaximumRateLimitDuration { get; set; }

        /// <summary>
        ///     Initialises a new <see cref="RestRequestOptionsBuilder"/>.
        /// </summary>
        public RestRequestOptionsBuilder()
        { }

        public RestRequestOptionsBuilder WithTimeout(TimeSpan timeout)
        {
            Timeout = timeout;
            return this;
        }

        public RestRequestOptionsBuilder WithCancellationToken(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
            return this;
        }

        public RestRequestOptionsBuilder WithReason(string reason)
        {
            Reason = reason;
            return this;
        }

        public RestRequestOptionsBuilder WithMaximumRateLimitDuration(TimeSpan maximumRateLimitDuration)
        {
            MaximumRateLimitDuration = maximumRateLimitDuration;
            return this;
        }

        public RestRequestOptions Build()
            => new RestRequestOptions(this);
    }
}
