using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalSlashCommandOptionChoiceExtensions
{
    public static TSlashCommandOptionChoice WithName<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice @this, string name)
        where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
    {
        @this.Name = name;
        return @this;
    }

    public static TSlashCommandOptionChoice AddNameLocalization<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice @this, CultureInfo locale, string nameLocalization)
        where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
    {
        Guard.IsNotNull(locale);
        Guard.IsNotNull(nameLocalization);

        if (@this.NameLocalizations.Add(locale, nameLocalization, out var dictionary))
            @this.NameLocalizations = new(dictionary);

        return @this;
    }

    public static TSlashCommandOptionChoice WithNameLocalizations<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice @this, IEnumerable<KeyValuePair<CultureInfo, string>> nameLocalizations)
        where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
    {
        Guard.IsNotNull(nameLocalizations);

        if (@this.NameLocalizations.With(nameLocalizations, out var dictionary))
            @this.NameLocalizations = new(dictionary);

        return @this;
    }

    public static TSlashCommandOptionChoice WithValue<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice @this, long value)
        where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
    {
        @this.Value = value;
        return @this;
    }

    public static TSlashCommandOptionChoice WithValue<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice @this, double value)
        where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
    {
        @this.Value = value;
        return @this;
    }

    public static TSlashCommandOptionChoice WithValue<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice @this, string value)
        where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
    {
        @this.Value = value;
        return @this;
    }
}
