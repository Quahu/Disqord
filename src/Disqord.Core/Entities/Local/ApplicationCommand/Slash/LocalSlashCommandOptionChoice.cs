using System.Collections.Generic;
using System.Globalization;
using Qommon;
using Qommon.Collections;

namespace Disqord;

public class LocalSlashCommandOptionChoice : ILocalConstruct<LocalSlashCommandOptionChoice>
{
    /// <summary>
    ///     Gets or sets the name of this choice.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<string> Name { get; set; }

    /// <summary>
    ///     Gets or sets the localizations of the name of this choice.
    /// </summary>
    public Optional<IDictionary<CultureInfo, string>> NameLocalizations { get; set; }

    /// <summary>
    ///     Gets or sets the value of this choice.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<object> Value { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSlashCommandOptionChoice"/>.
    /// </summary>
    public LocalSlashCommandOptionChoice()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalSlashCommandOptionChoice"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalSlashCommandOptionChoice(LocalSlashCommandOptionChoice other)
    {
        Name = other.Name;
        NameLocalizations = Optional.Convert(other.NameLocalizations, localizations => localizations.ToDictionary() as IDictionary<CultureInfo, string>);
        Value = other.Name;
    }

    /// <inheritdoc/>
    public virtual LocalSlashCommandOptionChoice Clone()
    {
        return new(this);
    }
}
