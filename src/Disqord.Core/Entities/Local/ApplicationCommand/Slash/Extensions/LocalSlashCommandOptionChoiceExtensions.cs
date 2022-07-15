using System.Collections.Generic;
using System.Globalization;
using Qommon;

namespace Disqord
{
    public static class LocalSlashCommandOptionChoiceExtensions
    {
        public static TSlashCommandOptionChoice WithName<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice @this, string name)
            where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
        {
            @this.Name = name;
            return @this;
        }

        public static TSlashCommandOptionChoice AddNameLocalization<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice @this, CultureInfo locale, string name)
            where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
        {
            Guard.IsNotNull(locale);
            Guard.IsNotNull(name);

            if (!@this.NameLocalizations.Add(locale, name, out var dictionary))
                @this.NameLocalizations = new(dictionary);

            return @this;
        }

        public static TSlashCommandOptionChoice WithNameLocalizations<TSlashCommandOptionChoice>(this TSlashCommandOptionChoice @this, IEnumerable<KeyValuePair<CultureInfo, string>> names)
            where TSlashCommandOptionChoice : LocalSlashCommandOptionChoice
        {
            Guard.IsNotNull(names);

            if (!@this.NameLocalizations.With(names, out var dictionary))
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
}
