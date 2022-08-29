using Disqord.Models;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents a local embed field.
/// </summary>
public class LocalEmbedField : ILocalConstruct<LocalEmbedField>, IJsonConvertible<EmbedFieldJsonModel>
{
    /// <summary>
    ///     Gets or sets the name of this field.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<string> Name { get; set; }

    /// <summary>
    ///     Gets or sets the value of this field.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<string> Value { get; set; }

    /// <summary>
    ///     Gets or sets whether this field is inline.
    /// </summary>
    public Optional<bool> IsInline { get; set; }

    /// <summary>
    ///     Gets the total text length of this field.
    /// </summary>
    public int Length
    {
        get
        {
            var nameLength = Name.GetValueOrDefault()?.Length ?? 0;
            var valueLength = Value.GetValueOrDefault()?.Length ?? 0;
            return nameLength + valueLength;
        }
    }

    /// <summary>
    ///     Instantiates a new <see cref="LocalEmbedField"/>.
    /// </summary>
    public LocalEmbedField()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalEmbedField"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalEmbedField(LocalEmbedField other)
    {
        Name = other.Name;
        Value = other.Value;
        IsInline = other.IsInline;
    }

    /// <inheritdoc/>
    public virtual LocalEmbedField Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public virtual EmbedFieldJsonModel ToModel()
    {
        OptionalGuard.HasValue(Name);
        OptionalGuard.HasValue(Value);

        return new EmbedFieldJsonModel
        {
            Name = Name.Value,
            Value = Value.Value,
            Inline = IsInline
        };
    }

    /// <summary>
    ///     Converts the specified embed field to a <see cref="LocalEmbedField"/>.
    /// </summary>
    /// <param name="field"> The embed field to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalEmbedField"/>.
    /// </returns>
    public static LocalEmbedField CreateFrom(IEmbedField field)
    {
        return new LocalEmbedField
        {
            Name = field.Name,
            Value = field.Value,
            IsInline = field.IsInline
        };
    }
}
