using Disqord.Models;

namespace Disqord
{
    public interface IPartialChannel : ISnowflakeEntity, INamable, IJsonUpdatable<ChannelJsonModel>
    {
        ChannelType Type { get; }

        ChannelPermissions Permissions { get; }
    }
}