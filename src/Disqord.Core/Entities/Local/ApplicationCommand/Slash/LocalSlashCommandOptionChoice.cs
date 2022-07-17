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

    public LocalSlashCommandOptionChoice()
    { }

    protected LocalSlashCommandOptionChoice(LocalSlashCommandOptionChoice other)
    {
        Name = other.Name;
        NameLocalizations = Optional.Convert(other.NameLocalizations, localizations => localizations.ToDictionary() as IDictionary<CultureInfo, string>);
        Value = other.Name;
    }

    public virtual LocalSlashCommandOptionChoice Clone()
    {
        return new(this);
    }
}
