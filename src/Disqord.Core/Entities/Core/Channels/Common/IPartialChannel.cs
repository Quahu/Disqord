using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a partial channel
    /// </summary>
    public interface IPartialChannel : ISnowflakeEntity, INamable, IJsonUpdatable<ChannelJsonModel>
    {
        /// <summary>
        ///     Gets the underlying type of this channel.
        /// </summary>
        ChannelType Type { get; }

        /// <summary>
        ///     Gets the permissions of the respective user in this channel.
        /// </summary>
        ChannelPermissions Permissions { get; }
    }
}