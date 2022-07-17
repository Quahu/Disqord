using System;

namespace Disqord.Gateway;

public class IntegrationCreatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the integration was created in.
    /// </summary>
    public Snowflake GuildId => Integration.GuildId;

    /// <summary>
    ///     Gets the created integration.
    /// </summary>
    public IIntegration Integration { get; }

    public IntegrationCreatedEventArgs(
        IIntegration integration)
    {
        Integration = integration;
    }
}