using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Qommon;
using Qommon.Collections;

namespace Disqord;

public class LocalSlashCommand : LocalApplicationCommand
{
    /// <summary>
    ///     Gets or sets the description of this command.
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
    ///     Gets or sets the options of this command.
    /// </summary>
    public Optional<IList<LocalSlashCommandOption>> Options { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSlashCommand"/>.
    /// </summary>
    public LocalSlashCommand()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSlashCommand"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalSlashCommand(LocalSlashCommand other)
        : base(other)
    {
        Description = other.Description;
        DescriptionLocalizations = Optional.Convert(other.DescriptionLocalizations, localizations => localizations.ToDictionary() as IDictionary<CultureInfo, string>);
        Options = Optional.Convert(other.Options, options => options.Select(option => option.Clone()).ToList() as IList<LocalSlashCommandOption>);
    }

    /// <inheritdoc/>
    public override LocalSlashCommand Clone()
    {
        return new(this);
    }
}
