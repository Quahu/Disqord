namespace Disqord
{
    public sealed class CreateTextChannelProperties : CreateNestedChannelProperties
    {
        public Optional<string> Topic { internal get; set; }

        public Optional<int> Slowmode { internal get; set; }

        public Optional<bool> IsNsfw { internal get; set; }

        internal CreateTextChannelProperties()
        { }
    }
}
