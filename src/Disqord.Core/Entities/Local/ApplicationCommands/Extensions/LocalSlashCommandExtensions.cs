using System;
using System.Collections.Generic;

namespace Disqord
{
    public static class LocalSlashCommandExtensions
    {
        public static TSlashCommand WithDescription<TSlashCommand>(this TSlashCommand command, string description)
            where TSlashCommand : LocalSlashCommand
        {
            command.Description = description;
            return command;
        }

        public static TSlashCommand WithOptions<TSlashCommand>(this TSlashCommand command, IEnumerable<LocalSlashCommandOption> options)
            where TSlashCommand : LocalSlashCommand
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            command._options.Clear();
            command._options.AddRange(options);
            return command;
        }
    }
}
