using System.ComponentModel;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalEmbedFieldExtensions
{
    public static TField WithName<TField>(this TField field, string name)
        where TField : LocalEmbedField
    {
        field.Name = name;
        return field;
    }

    public static TEmbedField WithBlankName<TEmbedField>(this TEmbedField field)
        where TEmbedField : LocalEmbedField
    {
        return field.WithName("\u200b");
    }

    public static TEmbedField WithValue<TEmbedField>(this TEmbedField field, object value)
        where TEmbedField : LocalEmbedField
    {
        var stringValue = value.ToString();
        Guard.IsNotNull(stringValue, "value.ToString()");
        field.Value = stringValue;
        return field;
    }

    public static TEmbedField WithValue<TEmbedField>(this TEmbedField field, string value)
        where TEmbedField : LocalEmbedField
    {
        field.Value = value;
        return field;
    }

    public static TEmbedField WithBlankValue<TEmbedField>(this TEmbedField field)
        where TEmbedField : LocalEmbedField
    {
        return field.WithValue("\u200b");
    }

    public static TEmbedField WithIsInline<TEmbedField>(this TEmbedField field, bool isInline = true)
        where TEmbedField : LocalEmbedField
    {
        field.IsInline = isInline;
        return field;
    }
}
