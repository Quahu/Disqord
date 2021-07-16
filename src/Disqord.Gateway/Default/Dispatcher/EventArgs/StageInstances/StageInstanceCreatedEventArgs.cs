using System;

namespace Disqord.Gateway
{
    public class StageInstanceCreatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild in which the stage instance was created.
        /// </summary>
        public Snowflake GuildId => StageInstance.GuildId;

        /// <summary>
        ///     Gets the ID of the channel in which the stage instance was created.
        /// </summary>
        public Snowflake ChannelId => StageInstance.ChannelId;

        /// <summary>
        ///     Gets the ID of the created stage instance.
        /// </summary>
        public Snowflake StageInstanceId => StageInstance.Id;

        /// <summary>
        ///     Gets the created stage instance.
        /// </summary>
        public IStageInstance StageInstance { get; }

        public StageInstanceCreatedEventArgs(IStageInstance stageInstance)
        {
            StageInstance = stageInstance;
        }
    }
}