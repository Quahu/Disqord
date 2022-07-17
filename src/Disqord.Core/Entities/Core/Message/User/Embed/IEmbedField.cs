namespace Disqord;

/// <summary>
///     Represents an embed field.
/// </summary>
public interface IEmbedField : INamableEntity
{
    /// <summary>
    ///     Gets the value of this field.
    /// </summary>
    string Value { get; }

    /// <summary>
    ///     Gets whether this field is inline.
    /// </summary>
    bool IsInline { get; }
}
