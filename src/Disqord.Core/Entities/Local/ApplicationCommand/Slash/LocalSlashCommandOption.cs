using System.Collections.Generic;
using System.Globalization;
using Qommon;

namespace Disqord;

public class LocalSlashCommandOption : ILocalConstruct<LocalSlashCommandOption>
{
    /// <summary>
    ///     Gets or sets the type of this option.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<SlashCommandOptionType> Type { get; set; }

    /// <summary>
    ///     Gets or sets the name of this option.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<string> Name { get; set; }

    /// <summary>
    ///     Gets or sets the localizations of the name of this command.
    /// </summary>
    public Optional<IDictionary<CultureInfo, string>> NameLocalizations { get; set; }

    /// <summary>
    ///     Gets or sets the description of this option/
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<string> Description { get; set; }

    /// <summary>
    ///     Gets or sets the localizations of the description of this command.
    /// </summary>
    public Optional<IDictionary<CultureInfo, string>> DescriptionLocalizations { get; set; }

    /// <summary>
    ///     Gets or sets whether this option is required.
    /// </summary>
    /// <remarks>
    ///     If not set, Discord defaults it to <see langword="false"/>.
    /// </remarks>
    public Optional<bool> IsRequired { get; set; }

    /// <summary>
    ///     Gets or sets the choices of this option.
    /// </summary>
    public Optional<IList<LocalSlashCommandOptionChoice>> Choices { get; set; }

    /// <summary>
    ///     Gets or sets the nested options of this option.
    /// </summary>
    public Optional<IList<LocalSlashCommandOption>> Options { get; set; }

    /// <summary>
    ///     Gets or sets the channel types of this option.
    /// </summary>
    public Optional<IList<ChannelType>> ChannelTypes { get; set; }

    /// <summary>
    ///     Gets or sets the minimum integer/number value this option allows.
    /// </summary>
    public Optional<double> MinimumValue { get; set; }

    /// <summary>
    ///     Gets or sets the maximum integer/number value this option allows.
    /// </summary>
    public Optional<double> MaximumValue { get; set; }

    /// <summary>
    ///     Gets or sets the minimum length of the value this option allows.
    /// </summary>
    public Optional<int> MinimumLength { get; set; }

    /// <summary>
    ///     Gets or sets the maximum length of the value this option allows.
    /// </summary>
    public Optional<int> MaximumLength { get; set; }

    /// <summary>
    ///     Gets or sets whether this option supports auto-complete.
    /// </summary>
    public Optional<bool> HasAutoComplete { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSlashCommandOption"/>.
    /// </summary>
    public LocalSlashCommandOption()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSlashCommandOption"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalSlashCommandOption(LocalSlashCommandOption other)
    {
        Guard.IsNotNull(other);

        Type = other.Type;
        Name = other.Name;
        Description = other.Description;
        IsRequired = other.IsRequired;
        Choices = other.Choices.DeepClone();
        Options = other.Options.DeepClone();
        ChannelTypes = other.ChannelTypes.Clone();
        MinimumValue = other.MinimumValue;
        MaximumValue = other.MaximumValue;
        MinimumLength = other.MinimumLength;
        MaximumLength = other.MaximumLength;
        HasAutoComplete = other.HasAutoComplete;
    }

    /// <inheritdoc/>
    public virtual LocalSlashCommandOption Clone()
    {
        return new(this);
    }
}
