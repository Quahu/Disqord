using System;
using Disqord.Models;
using Qommon;

namespace Disqord
{
    /// <inheritdoc cref="IForumChannel"/>
    public class TransientForumChannel : TransientCategorizableGuildChannel, IForumChannel
    {
        /// <inheritdoc/>
        public string Topic => Model.Topic.Value;

        /// <inheritdoc/>
        public bool IsNsfw => Model.Nsfw.Value;

        /// <inheritdoc/>
        public TimeSpan DefaultAutomaticArchiveDuration => TimeSpan.FromMinutes(Model.DefaultAutoArchiveDuration.GetValueOrDefault(1400));

        /// <inheritdoc/>
        public TimeSpan Slowmode => TimeSpan.FromSeconds(Model.RateLimitPerUser.GetValueOrDefault());

        /// <inheritdoc/>
        public Snowflake? LastThreadId => Model.LastMessageId.GetValueOrDefault();

        public TransientForumChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
