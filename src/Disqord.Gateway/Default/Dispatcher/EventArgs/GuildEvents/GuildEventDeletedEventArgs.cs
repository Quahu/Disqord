using System;

namespace Disqord.Gateway;

public class GuildEventDeletedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the event was deleted.
    /// </summary>
    public Snowflake GuildId => Event.GuildId;

    /// <summary>
    ///     Gets the ID of the deleted event.
    /// </summary>
    public Snowflake EventId => Event.Id;

    /// <summary>
    ///     Gets the deleted event.
    /// </summary>
    public IGuildEvent Event { get; }

    public GuildEventDeletedEventArgs(
        IGuildEvent @event)
    {
        Event = @event;
    }
}
