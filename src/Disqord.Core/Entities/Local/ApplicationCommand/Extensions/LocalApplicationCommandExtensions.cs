using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalApplicationCommandExtensions
{
    public static TApplicationCommand WithName<TApplicationCommand>(this TApplicationCommand command, string name)
        where TApplicationCommand : LocalApplicationCommand
    {
        command.Name = name;
        return command;
    }

    public static TApplicationCommand AddNameLocalization<TApplicationCommand>(this TApplicationCommand @this, CultureInfo locale, string nameLocalization)
        where TApplicationCommand : LocalApplicationCommand
    {
        Guard.IsNotNull(locale);
        Guard.IsNotNull(nameLocalization);

        if (@this.NameLocalizations.Add(locale, nameLocalization, out var dictionary))
            @this.NameLocalizations = new(dictionary);

        return @this;
    }

    public static TApplicationCommand WithNameLocalizations<TApplicationCommand>(this TApplicationCommand @this, IEnumerable<KeyValuePair<CultureInfo, string>> nameLocalizations)
        where TApplicationCommand : LocalApplicationCommand
    {
        Guard.IsNotNull(nameLocalizations);

        if (@this.NameLocalizations.With(nameLocalizations, out var dictionary))
            @this.NameLocalizations = new(dictionary);

        return @this;
    }

    public static TApplicationCommand WithIsEnabledByDefault<TApplicationCommand>(this TApplicationCommand command, bool isEnabledByDefault = true)
        where TApplicationCommand : LocalApplicationCommand
    {
        command.IsEnabledByDefault = isEnabledByDefault;
        return command;
    }
}
