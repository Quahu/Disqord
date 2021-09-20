using System;
using System.Collections.Generic;

namespace Disqord
{
    public static class LocalSlashCommandExtensions
    {
        public static TApplicationCommand WithDescription<TApplicationCommand>(this TApplicationCommand command, string description)
            where TApplicationCommand : LocalSlashCommand
        {
            command.Description = description;
            return command;
        }

        public static TApplicationCommand WithOptions<TApplicationCommand>(this TApplicationCommand command, IEnumerable<LocalSlashCommandOption> options)
            where TApplicationCommand : LocalSlashCommand
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            command._options.Clear();
            command._options.AddRange(options);
            return command;
        }
    }
}
