using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalSelectionComponentOption : ILocalConstruct<LocalSelectionComponentOption>, IJsonConvertible<SelectOptionJsonModel>
{
    /// <summary>
    ///     Gets or sets the label of this option.
    /// </summary>
    public Optional<string> Label { get; set; }

    /// <summary>
    ///     Gets or sets the label of this option.
    /// </summary>
    public Optional<string> Value { get; set; }

    /// <summary>
    ///     Gets or sets the description of this option.
    /// </summary>
    public Optional<string> Description { get; set; }

    /// <summary>
    ///     Gets or sets the emoji of this option.
    /// </summary>
    public Optional<LocalEmoji> Emoji { get; set; }

    /// <summary>
    ///     Gets or sets whether this option is the default.
    /// </summary>
    public Optional<bool> IsDefault { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSelectionComponentOption"/>.
    /// </summary>
    public LocalSelectionComponentOption()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSelectionComponentOption"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalSelectionComponentOption(LocalSelectionComponentOption other)
    {
        Label = other.Label;
        Value = other.Value;
        Description = other.Description;
        Emoji = other.Emoji;
        IsDefault = other.IsDefault;
    }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSelectionComponentOption"/>.
    /// </summary>
    /// <param name="label"> The label of the option. </param>
    /// <param name="value"> The value of the option. </param>
    public LocalSelectionComponentOption(string label, string value)
    {
        Label = label;
        Value = value;
    }

    /// <inheritdoc/>
    public virtual LocalSelectionComponentOption Clone()
    {
        return new(this);
    }

    /// <inheritdoc/>
    public virtual SelectOptionJsonModel ToModel()
    {
        return new SelectOptionJsonModel
        {
            Label = Label,
            Value = Value,
            Description = Description,
            Emoji = Optional.Convert(Emoji, emoji => emoji.ToModel()),
            Default = IsDefault
        };
    }

    /// <summary>
    ///     Converts the specified selection component option to a <see cref="LocalSelectionComponentOption"/>.
    /// </summary>
    /// <param name="selectionComponentOption"> The selection component option to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalSelectionComponentOption"/>.
    /// </returns>
    public static LocalSelectionComponentOption CreateFrom(ISelectionComponentOption selectionComponentOption)
    {
        return new LocalSelectionComponentOption
        {
            Label = selectionComponentOption.Label,
            Value = selectionComponentOption.Value,
            Description = Optional.FromNullable(selectionComponentOption.Description),
            Emoji = Optional.Conditional(selectionComponentOption.Emoji != null, LocalEmoji.FromEmoji, selectionComponentOption.Emoji)!,
            IsDefault = selectionComponentOption.IsDefault
        };
    }
}
