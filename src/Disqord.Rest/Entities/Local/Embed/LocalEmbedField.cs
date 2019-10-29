namespace Disqord
{
    public sealed class LocalEmbedField
    {
        public string Name { get; }

        public string Value { get; }

        public bool IsInline { get; }

        internal LocalEmbedField(LocalEmbedFieldBuilder builder)
        {
            Name = builder.Name;
            Value = builder.Value;
            IsInline = builder.IsInline;
        }
    }
}
