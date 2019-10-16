namespace Disqord
{
    public class ModifyGroupChannelProperties : ModifyChannelProperties
    {
        public Optional<LocalAttachment> Icon { internal get; set; }

        internal ModifyGroupChannelProperties()
        { }
    }
}
