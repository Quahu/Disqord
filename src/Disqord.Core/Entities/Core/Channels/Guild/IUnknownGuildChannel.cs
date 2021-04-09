namespace Disqord
{
    /// <summary>
    ///     Represents an unknown guild channel.
    /// </summary>
    public interface IUnknownGuildChannel : INestableChannel
    {
        /// <summary>
        ///     Gets the <see cref="ChannelType"/> of this channel.
        /// </summary>
        ChannelType Type { get; }
    }
}
