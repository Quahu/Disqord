using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest.Repetition
{
    /// <summary>
    ///     Represents a repeater that intervally triggers typing in a channel.
    /// </summary>
    public class TypingRepeater : Repeater
    {
        /// <inheritdoc/>
        public override TimeSpan Interval => TimeSpan.FromSeconds(7.5);

        /// <summary>
        ///     Gets the channel ID to trigger typing in.
        /// </summary>
        public Snowflake ChannelId { get; }

        /// <summary>
        ///     Instantiates a new <see cref="TypingRepeater"/>.
        /// </summary>
        /// <param name="client"> The client to execute the requests with. </param>
        /// <param name="channelId"> The channel to trigger typing in. </param>
        /// <param name="options"> The optional request options. </param>
        public TypingRepeater(IRestClient client, Snowflake channelId, IRestRequestOptions options = null)
            : base(client, options)
        {
            if (options != null && options.CancellationToken != default)
                throw new ArgumentException("The options' cancellation token is not supported.", nameof(options));

            ChannelId = channelId;
        }

        /// <inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (Options != null)
                Options.CancellationToken = cancellationToken;

            return Client.TriggerTypingAsync(ChannelId, Options);
        }
    }
}
