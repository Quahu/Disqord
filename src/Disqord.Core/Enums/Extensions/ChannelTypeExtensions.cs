using System.ComponentModel;

namespace Disqord.Enums.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ChannelTypeExtensions
    {
        public static bool IsThread(this ChannelType type)
            => type is ChannelType.NewsThread or ChannelType.PublicThread or ChannelType.PublicThread;
    }
}
