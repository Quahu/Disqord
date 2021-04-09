namespace Disqord
{
    public class ModifyTextChannelActionProperties : ModifyNestableChannelActionProperties
    {
        public Optional<string> Topic { internal get; set; }

        public Optional<bool> IsNsfw { internal get; set; }

        public Optional<int> Slowmode { internal get; set; }

        internal ModifyTextChannelActionProperties()
        { }
    }
}
