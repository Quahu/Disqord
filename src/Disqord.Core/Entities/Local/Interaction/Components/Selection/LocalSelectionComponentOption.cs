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

    public LocalSelectionComponentOption()
    { }

    public LocalSelectionComponentOption(string label, string value)
    {
        Label = label;
        Value = value;
    }

    protected LocalSelectionComponentOption(LocalSelectionComponentOption other)
    {
        Label = other.Label;
        Value = other.Value;
        Description = other.Description;
        Emoji = other.Emoji;
        IsDefault = other.IsDefault;
    }

    public LocalSelectionComponentOption Clone()
    {
        return new(this);
    }

    /// <inheritdoc />
    public SelectOptionJsonModel ToModel()
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
}
