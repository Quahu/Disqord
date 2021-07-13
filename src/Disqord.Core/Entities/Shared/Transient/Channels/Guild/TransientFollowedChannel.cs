﻿using Disqord.Models;

namespace Disqord
{
    public class TransientFollowedChannel : TransientEntity<FollowedChannelJsonModel>, IFollowedChannel
    {
        /// <inheritdoc/>
        public Snowflake ChannelId => Model.ChannelId;

        /// <inheritdoc/>
        public Snowflake WebhookId => Model.WebhookId;

        public TransientFollowedChannel(IClient client, FollowedChannelJsonModel model)
            : base(client, model)
        { }
    }
}
