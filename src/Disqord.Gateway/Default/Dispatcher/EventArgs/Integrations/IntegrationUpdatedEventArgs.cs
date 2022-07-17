using System;

namespace Disqord.Gateway;

public class IntegrationUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the integration was updated in.
    /// </summary>
    public Snowflake GuildId => Integration.GuildId;

    /// <summary>
    ///     Gets the updated integration.
    /// </summary>
    public IIntegration Integration { get; }

    public IntegrationUpdatedEventArgs(
        IIntegration integration)
    {
        Integration = integration;
    }
}