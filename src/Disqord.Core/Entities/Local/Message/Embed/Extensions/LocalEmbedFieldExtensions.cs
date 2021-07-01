namespace Disqord
{
    public static class LocalEmbedFieldExtensions
    {
        public static LocalEmbedField WithName(this LocalEmbedField field, string name)
        {
            field.Name = name;
            return field;
        }

        public static LocalEmbedField WithBlankName(this LocalEmbedField field)
            => field.WithName("\u200b");

        public static LocalEmbedField WithValue(this LocalEmbedField field, string value)
        {
            field.Value = value;
            return field;
        }

        public static LocalEmbedField WithValue(this LocalEmbedField field, object value)
        {
            field.Value = value?.ToString();
            return field;
        }

        public static LocalEmbedField WithBlankValue(this LocalEmbedField field)
            => field.WithValue("\u200b");

        public static LocalEmbedField WithIsInline(this LocalEmbedField field, bool isInline)
        {
            field.IsInline = isInline;
            return field;
        }
    }
}
