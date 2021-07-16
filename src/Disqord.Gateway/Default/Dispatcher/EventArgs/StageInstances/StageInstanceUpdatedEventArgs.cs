using System;

namespace Disqord.Gateway
{
    public class StageInstanceUpdatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild in which the stage instance was updated.
        /// </summary>
        public Snowflake GuildId => NewStageInstance.GuildId;

        /// <summary>
        ///     Gets the ID of the channel in which the stage instance was updated.
        /// </summary>
        public Snowflake ChannelId => NewStageInstance.ChannelId;

        /// <summary>
        ///     Gets the ID of the updated stage instance.
        /// </summary>
        public Snowflake StageInstanceId => NewStageInstance.Id;

        /// <summary>
        ///     Gets the stage instance in the state before the update occurred.
        ///     Returns <see langword="null"/> if the stage instance was not cached.
        /// </summary>
        public CachedStageInstance OldStageInstance { get; }

        /// <summary>
        ///     Gets the updated stage instance.
        /// </summary>
        public IStageInstance NewStageInstance { get; }

        public StageInstanceUpdatedEventArgs(CachedStageInstance oldStageInstance, IStageInstance newStageInstance)
        {
            OldStageInstance = oldStageInstance;
            NewStageInstance = newStageInstance;
        }
    }
}