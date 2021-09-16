using System;
using System.Collections.Generic;

namespace Disqord
{
    public static class LocalApplicationCommandExtensions
    {
        public static TApplicationCommand WithType<TApplicationCommand>(this TApplicationCommand command, ApplicationCommandType type)
            where TApplicationCommand : LocalApplicationCommand
        {
            command.Type = type;
            return command;
        }

        public static TApplicationCommand WithName<TApplicationCommand>(this TApplicationCommand command, string name)
            where TApplicationCommand : LocalApplicationCommand
        {
            command.Name = name;
            return command;
        }

        public static TApplicationCommand WithDescription<TApplicationCommand>(this TApplicationCommand command, string description)
            where TApplicationCommand : LocalApplicationCommand
        {
            command.Description = description;
            return command;
        }

        public static TApplicationCommand WithIsEnabledByDefault<TApplicationCommand>(this TApplicationCommand command, bool isEnabledByDefault)
            where TApplicationCommand : LocalApplicationCommand
        {
            command.IsEnabledByDefault = isEnabledByDefault;
            return command;
        }

        public static TApplicationCommand WithOptions<TApplicationCommand>(this TApplicationCommand command, IEnumerable<LocalApplicationCommandOption> options)
            where TApplicationCommand : LocalApplicationCommand
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            command._options.Clear();
            command._options.AddRange(options);
            return command;
        }
    }
}
