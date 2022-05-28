using System.Collections.Generic;
using System.Globalization;
using Qommon;

namespace Disqord
{
    public static class LocalApplicationCommandExtensions
    {
        public static TApplicationCommand WithName<TApplicationCommand>(this TApplicationCommand command, string name)
            where TApplicationCommand : LocalApplicationCommand
        {
            command.Name = name;
            return command;
        }

        public static TApplicationCommand AddNameLocalization<TApplicationCommand>(this TApplicationCommand @this, CultureInfo locale, string name)
            where TApplicationCommand : LocalApplicationCommand
        {
            Guard.IsNotNull(locale);
            Guard.IsNotNull(name);

            if (!@this.NameLocalizations.Add(locale, name, out var dictionary))
                @this.NameLocalizations = new(dictionary);

            return @this;
        }

        public static TApplicationCommand WithNameLocalizations<TApplicationCommand>(this TApplicationCommand @this, IEnumerable<KeyValuePair<CultureInfo, string>> names)
            where TApplicationCommand : LocalApplicationCommand
        {
            Guard.IsNotNull(names);

            if (!@this.NameLocalizations.With(names, out var dictionary))
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
}
