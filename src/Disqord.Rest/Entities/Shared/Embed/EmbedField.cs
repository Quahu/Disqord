namespace Disqord
{
    public sealed class EmbedField
    {
        public string Name { get; internal set; }

        public string Value { get; internal set; }

        public bool IsInline { get; internal set; }

        internal EmbedField()
        { }
    }
}
