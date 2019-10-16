namespace Disqord
{
    public class ModifyTextChannelProperties : ModifyNestedChannelProperties
    {
        public Optional<string> Topic { internal get; set; }

        public Optional<bool> IsNsfw { internal get; set; }

        public Optional<int> Slowmode { internal get; set; }

        internal ModifyTextChannelProperties()
        { }
    }
}
