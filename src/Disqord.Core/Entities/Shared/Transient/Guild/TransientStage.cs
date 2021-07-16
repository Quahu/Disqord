﻿using Disqord.Models;

namespace Disqord
{
    /// <inheritdoc cref="IStage"/>/>
    public class TransientStage : TransientEntity<StageInstanceJsonModel>, IStage
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public Snowflake GuildId => Model.GuildId;

        /// <inheritdoc/>
        public Snowflake ChannelId => Model.ChannelId;

        /// <inheritdoc/>
        public string Topic => Model.Topic;

        /// <inheritdoc/>
        public StagePrivacyLevel PrivacyLevel => Model.PrivacyLevel;

        /// <inheritdoc/>
        public bool IsDiscoveryDisabled => Model.DiscoverableDisabled;

        public TransientStage(IClient client, StageInstanceJsonModel model)
            : base(client, model)
        { }
    }
}
