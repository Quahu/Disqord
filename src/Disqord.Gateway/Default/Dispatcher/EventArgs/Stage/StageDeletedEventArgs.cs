using System;

namespace Disqord.Gateway;

public class StageDeletedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the stage was deleted.
    /// </summary>
    public Snowflake GuildId => Stage.GuildId;

    /// <summary>
    ///     Gets the ID of the channel in which the stage was deleted.
    /// </summary>
    public Snowflake ChannelId => Stage.ChannelId;

    /// <summary>
    ///     Gets the ID of the deleted stage.
    /// </summary>
    public Snowflake StageId => Stage.Id;

    /// <summary>
    ///     Gets the deleted stage.
    /// </summary>
    public IStage Stage { get; }

    public StageDeletedEventArgs(
        IStage stage)
    {
        Stage = stage;
    }
}