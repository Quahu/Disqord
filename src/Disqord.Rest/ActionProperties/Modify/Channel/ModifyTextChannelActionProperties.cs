namespace Disqord
{
    public class ModifyTextChannelActionProperties : ModifyMessageGuildChannelActionProperties
    {
        public Optional<string> Topic { internal get; set; }

        public Optional<bool> IsNsfw { internal get; set; }

        internal ModifyTextChannelActionProperties()
        { }
    }
}
