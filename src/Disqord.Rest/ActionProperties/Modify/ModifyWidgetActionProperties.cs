using Qommon;

namespace Disqord
{
    public sealed class ModifyWidgetActionProperties
    {
        public Optional<bool> IsEnabled { internal get; set; }

        public Optional<Snowflake?> ChannelId { internal get; set; }

        internal ModifyWidgetActionProperties()
        { }
    }
}
