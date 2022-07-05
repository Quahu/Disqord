namespace Disqord
{
    /// <summary>
    ///     Represents a guild forum channel.
    /// </summary>
    public interface IForumChannel : ITopicChannel, IThreadParentChannel, ISlowmodeChannel
    {
        /// <summary>
        ///     Gets the ID of the last thread created in this channel.
        /// </summary>
        Snowflake? LastThreadId { get; }
    }
}
