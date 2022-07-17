using System;

namespace Disqord.Gateway;

public class IntegrationsUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the integrations were updated in.
    /// </summary>
    public Snowflake GuildId { get; }

    public IntegrationsUpdatedEventArgs(
        Snowflake guildId)
    {
        GuildId = guildId;
    }
}