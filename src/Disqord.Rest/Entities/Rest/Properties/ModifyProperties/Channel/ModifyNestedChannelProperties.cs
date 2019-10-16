namespace Disqord
{
    public abstract class ModifyNestedChannelProperties : ModifyGuildChannelProperties
    {
        public Optional<ulong> CategoryId { internal get; set; }

        internal ModifyNestedChannelProperties()
        { }
    }
}
