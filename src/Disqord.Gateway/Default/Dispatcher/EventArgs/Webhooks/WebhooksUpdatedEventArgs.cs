using System;

namespace Disqord.Gateway;

public class WebhooksUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the webhooks were updated in.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the ID of the channel the webhooks were updated in.
    /// </summary>
    public Snowflake ChannelId { get; }

    public WebhooksUpdatedEventArgs(
        Snowflake guildId,
        Snowflake channelId)
    {
        GuildId = guildId;
        ChannelId = channelId;
    }
}