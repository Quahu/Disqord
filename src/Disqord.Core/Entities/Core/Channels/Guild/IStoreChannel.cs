namespace Disqord
{
    /// <summary>
    ///     Represents a guild store channel.
    /// </summary>
    public interface IStoreChannel : ICategorizableGuildChannel
    {
        /// <summary>
        ///     Gets whether this store channel is not safe for work.
        /// </summary>
        bool IsNsfw { get; }
    }
}
