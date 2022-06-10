using System;
using System.ComponentModel;
using Disqord.Models;
using Qommon;

namespace Disqord.Gateway
{
    /// <inheritdoc cref="IForumChannel"/>
    public class CachedForumChannel : CachedCategorizableGuildChannel, IForumChannel
    {
        /// <inheritdoc/>
        public string Topic { get; private set; }

        /// <inheritdoc/>
        public bool IsAgeRestricted { get; private set; }

        /// <inheritdoc/>
        public TimeSpan DefaultAutomaticArchiveDuration { get; private set; }

        /// <inheritdoc/>
        public TimeSpan Slowmode { get; private set; }

        /// <inheritdoc/>
        public Snowflake? LastThreadId { get; private set; }

        public CachedForumChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model)
        { }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Update(ChannelJsonModel model)
        {
            base.Update(model);

            if (model.Topic.HasValue)
                Topic = model.Topic.Value;

            if (model.Nsfw.HasValue)
                IsAgeRestricted = model.Nsfw.Value;

            DefaultAutomaticArchiveDuration = TimeSpan.FromMinutes(model.DefaultAutoArchiveDuration.GetValueOrDefault(1440));

            if (model.RateLimitPerUser.HasValue)
                Slowmode = TimeSpan.FromSeconds(model.RateLimitPerUser.Value);

            if (model.LastMessageId.HasValue)
                LastThreadId = model.LastMessageId.Value;
        }
    }
}
