using System;
using Disqord.Models;

namespace Disqord
{
    /// <inheritdoc cref="IMessageGuildChannel"/>
    public abstract class TransientMessageGuildChannel : TransientCategorizableGuildChannel, IMessageGuildChannel
    {
        /// <inheritdoc/>
        public TimeSpan Slowmode => TimeSpan.FromSeconds(Model.RateLimitPerUser.GetValueOrDefault());

        /// <inheritdoc/>
        public Snowflake? LastMessageId => Model.LastMessageId.Value;

        /// <inheritdoc/>
        public DateTimeOffset? LastPinTimestamp => Model.LastPinTimestamp.Value;

        /// <inheritdoc/>
        public string Mention => Disqord.Mention.Channel(this);

        /// <inheritdoc/>
        public string Tag => $"#{Name}";

        protected TransientMessageGuildChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
