using System;

namespace Disqord.Gateway;

public class IntegrationDeletedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the integration was deleted in.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the ID of the deleted integration.
    /// </summary>
    public Snowflake IntegrationId { get; }

    /// <summary>
    ///     Gets the optional ID of the application attached to the deleted integration.
    /// </summary>
    public Snowflake? ApplicationId { get; }

    public IntegrationDeletedEventArgs(
        Snowflake guildId,
        Snowflake integrationId,
        Snowflake? applicationId)
    {
        GuildId = guildId;
        IntegrationId = integrationId;
        ApplicationId = applicationId;
    }
}