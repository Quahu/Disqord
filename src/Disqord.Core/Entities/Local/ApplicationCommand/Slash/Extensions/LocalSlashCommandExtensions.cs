using System.Collections.Generic;
using Qommon;

namespace Disqord
{
    public static class LocalSlashCommandExtensions
    {
        public static TSlashCommand WithDescription<TSlashCommand>(this TSlashCommand @this, string description)
            where TSlashCommand : LocalSlashCommand
        {
            @this.Description = description;
            return @this;
        }

        public static TSlashCommand AddOption<TSlashCommand>(this TSlashCommand @this, LocalSlashCommandOption option)
            where TSlashCommand : LocalSlashCommand
        {
            Guard.IsNotNull(option);

            if (!@this.Options.TryGetValue(out var list) || list == null)
            {
                list = new List<LocalSlashCommandOption>();
                @this.Options = new(list);
            }

            list.Add(option);
            return @this;
        }

        public static TSlashCommand WithOptions<TSlashCommand>(this TSlashCommand @this, IEnumerable<LocalSlashCommandOption> options)
            where TSlashCommand : LocalSlashCommand
        {
            Guard.IsNotNull(options);

            if (!@this.Options.TryGetValue(out var list) || list == null)
            {
                list = new List<LocalSlashCommandOption>(options);
                @this.Options = new(list);
                return @this;
            }

            list.Clear();
            foreach (var option in options)
                list.Add(option);

            return @this;
        }

        public static TSlashCommand WithOptions<TSlashCommand>(this TSlashCommand @this, params LocalSlashCommandOption[] options)
            where TSlashCommand : LocalSlashCommand
            => @this.WithOptions(options as IEnumerable<LocalSlashCommandOption>);
    }
}
