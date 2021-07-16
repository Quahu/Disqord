using System;

namespace Disqord.Gateway
{
    public class StageUpdatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild in which the stage instance was updated.
        /// </summary>
        public Snowflake GuildId => NewStage.GuildId;

        /// <summary>
        ///     Gets the ID of the channel in which the stage instance was updated.
        /// </summary>
        public Snowflake ChannelId => NewStage.ChannelId;

        /// <summary>
        ///     Gets the ID of the updated stage instance.
        /// </summary>
        public Snowflake StageId => NewStage.Id;

        /// <summary>
        ///     Gets the stage instance in the state before the update occurred.
        ///     Returns <see langword="null"/> if the stage instance was not cached.
        /// </summary>
        public CachedStage OldStage { get; }

        /// <summary>
        ///     Gets the updated stage instance.
        /// </summary>
        public IStage NewStage { get; }

        public StageUpdatedEventArgs(CachedStage oldStage, IStage newStage)
        {
            OldStage = oldStage;
            NewStage = newStage;
        }
    }
}