using Disqord.Models;

namespace Disqord.Gateway
{
    /// <inheritdoc cref="IStageInstance"/>
    public class CachedStageInstance : CachedSnowflakeEntity, IStageInstance
    {
        /// <inheritdoc/>
        public Snowflake GuildId { get; }

        /// <inheritdoc/>
        public Snowflake ChannelId { get; }

        /// <inheritdoc/>
        public string Topic { get; private set; }

        /// <inheritdoc/>
        public StagePrivacyLevel PrivacyLevel { get; private set; }

        /// <inheritdoc/>
        public bool IsDiscoveryDisabled { get; private set; }

        public CachedStageInstance(IGatewayClient client, StageInstanceJsonModel model)
            : base(client, model.Id)
        {
            GuildId = model.GuildId;
            ChannelId = model.ChannelId;

            Update(model);
        }

        public void Update(StageInstanceJsonModel model)
        {
            Topic = model.Topic;
            PrivacyLevel = model.PrivacyLevel;
            IsDiscoveryDisabled = model.DiscoverableDisabled;
        }
    }
}