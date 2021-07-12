using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a channel.
    /// </summary>
    public interface IChannel : ISnowflakeEntity, INamable, IJsonUpdatable<ChannelJsonModel>
    {
        /// <summary>
        ///     Gets the underlying type of this channel.
        /// </summary>
        ChannelType Type { get; }
    }
}
