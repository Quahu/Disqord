using System;

namespace Disqord.Gateway
{
    public class StageInstanceDeletedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild in which the stage instance was deleted.
        /// </summary>
        public Snowflake GuildId => StageInstance.GuildId;

        /// <summary>
        ///     Gets the ID of the channel in which the stage instance was deleted.
        /// </summary>
        public Snowflake ChannelId => StageInstance.ChannelId;

        /// <summary>
        ///     Gets the ID of the deleted stage instance.
        /// </summary>
        public Snowflake StageInstanceId => StageInstance.Id;

        /// <summary>
        ///     Gets the deleted stage instance.
        /// </summary>
        public IStageInstance StageInstance { get; }

        public StageInstanceDeletedEventArgs(IStageInstance stageInstance)
        {
            StageInstance = stageInstance;
        }
    }
}