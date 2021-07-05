namespace Disqord
{
    public static class LocalEmbedFieldExtensions
    {
        public static TEmbedField WithName<TEmbedField>(this TEmbedField field, string name)
            where TEmbedField : LocalEmbedField
        {
            field.Name = name;
            return field;
        }

        public static TEmbedField WithBlankName<TEmbedField>(this TEmbedField field)
            where TEmbedField : LocalEmbedField
            => field.WithName("\u200b");

        public static TEmbedField WithValue<TEmbedField>(this TEmbedField field, string value)
            where TEmbedField : LocalEmbedField
        {
            field.Value = value;
            return field;
        }

        public static TEmbedField WithValue<TEmbedField>(this TEmbedField field, object value)
            where TEmbedField : LocalEmbedField
        {
            field.Value = value?.ToString();
            return field;
        }

        public static TEmbedField WithBlankValue<TEmbedField>(this TEmbedField field)
            where TEmbedField : LocalEmbedField
            => field.WithValue("\u200b");

        public static TEmbedField WithIsInline<TEmbedField>(this TEmbedField field, bool isInline)
            where TEmbedField : LocalEmbedField
        {
            field.IsInline = isInline;
            return field;
        }
    }
}
