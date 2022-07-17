using Disqord.Models;

namespace Disqord.Gateway;

/// <inheritdoc cref="IStage"/>
public class CachedStage : CachedSnowflakeEntity, IStage
{
    /// <inheritdoc/>
    public Snowflake GuildId { get; }

    /// <inheritdoc/>
    public Snowflake ChannelId { get; }

    /// <inheritdoc/>
    public string Topic { get; private set; } = null!;

    /// <inheritdoc/>
    public PrivacyLevel PrivacyLevel { get; private set; }

    /// <inheritdoc/>
    public bool IsDiscoveryDisabled { get; private set; }

    /// <inheritdoc/>
    public Snowflake? GuildEventId { get; }

    public CachedStage(IGatewayClient client, StageInstanceJsonModel model)
        : base(client, model.Id)
    {
        GuildId = model.GuildId;
        ChannelId = model.ChannelId;
        GuildEventId = model.GuildScheduledEventId;

        Update(model);
    }

    public void Update(StageInstanceJsonModel model)
    {
        Topic = model.Topic;
        PrivacyLevel = model.PrivacyLevel;
        IsDiscoveryDisabled = model.DiscoverableDisabled;
    }
}
