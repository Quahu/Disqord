using System;

namespace Disqord.Gateway;

public class StageCreatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the stage was created.
    /// </summary>
    public Snowflake GuildId => Stage.GuildId;

    /// <summary>
    ///     Gets the ID of the channel in which the stage was created.
    /// </summary>
    public Snowflake ChannelId => Stage.ChannelId;

    /// <summary>
    ///     Gets the ID of the created stage.
    /// </summary>
    public Snowflake StageId => Stage.Id;

    /// <summary>
    ///     Gets the created stage.
    /// </summary>
    public IStage Stage { get; }

    public StageCreatedEventArgs(
        IStage stage)
    {
        Stage = stage;
    }
}