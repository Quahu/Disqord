namespace Disqord
{
    public sealed class ModifyWidgetProperties
    {
        public Optional<bool> IsEnabled { internal get; set; }

        public Optional<ulong?> ChannelId { internal get; set; }

        internal ModifyWidgetProperties()
        { }
    }
}
