using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Qommon;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalSlashCommandExtensions
{
    public static TSlashCommand WithDescription<TSlashCommand>(this TSlashCommand @this, string description)
        where TSlashCommand : LocalSlashCommand
    {
        @this.Description = description;
        return @this;
    }

    public static TSlashCommand AddDescriptionLocalization<TSlashCommand>(this TSlashCommand @this, CultureInfo locale, string descriptionLocalization)
        where TSlashCommand : LocalSlashCommand
    {
        Guard.IsNotNull(locale);
        Guard.IsNotNull(descriptionLocalization);

        if (@this.DescriptionLocalizations.Add(locale, descriptionLocalization, out var dictionary))
            @this.DescriptionLocalizations = new(dictionary);

        return @this;
    }

    public static TSlashCommand WithDescriptionLocalizations<TSlashCommand>(this TSlashCommand @this, IEnumerable<KeyValuePair<CultureInfo, string>> descriptionLocalizations)
        where TSlashCommand : LocalSlashCommand
    {
        Guard.IsNotNull(descriptionLocalizations);

        if (@this.DescriptionLocalizations.With(descriptionLocalizations, out var dictionary))
            @this.DescriptionLocalizations = new(dictionary);

        return @this;
    }

    public static TSlashCommand AddOption<TSlashCommand>(this TSlashCommand @this, LocalSlashCommandOption option)
        where TSlashCommand : LocalSlashCommand
    {
        Guard.IsNotNull(option);

        if (@this.Options.Add(option, out var list))
            @this.Options = new(list);

        return @this;
    }

    public static TSlashCommand WithOptions<TSlashCommand>(this TSlashCommand @this, IEnumerable<LocalSlashCommandOption> options)
        where TSlashCommand : LocalSlashCommand
    {
        Guard.IsNotNull(options);

        if (@this.Options.With(options, out var list))
            @this.Options = new(list);

        return @this;
    }

    public static TSlashCommand WithOptions<TSlashCommand>(this TSlashCommand @this, params LocalSlashCommandOption[] options)
        where TSlashCommand : LocalSlashCommand
    {
        return @this.WithOptions(options as IEnumerable<LocalSlashCommandOption>);
    }
}
