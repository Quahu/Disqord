using System;
using System.Collections.Generic;

namespace Disqord
{
    public static class LocalSlashCommandOptionExtensions
    {
        public static TApplicationCommandOption WithType<TApplicationCommandOption>(this TApplicationCommandOption option, SlashCommandOptionType type)
            where TApplicationCommandOption : LocalSlashCommandOption
        {
            option.Type = type;
            return option;
        }

        public static TApplicationCommandOption WithName<TApplicationCommandOption>(this TApplicationCommandOption option, string name)
            where TApplicationCommandOption : LocalSlashCommandOption
        {
            option.Name = name;
            return option;
        }

        public static TApplicationCommandOption WithDescription<TApplicationCommandOption>(this TApplicationCommandOption option, string description)
            where TApplicationCommandOption : LocalSlashCommandOption
        {
            option.Description = description;
            return option;
        }

        public static TApplicationCommandOption WithIsRequired<TApplicationCommandOption>(this TApplicationCommandOption option, bool isRequired)
            where TApplicationCommandOption : LocalSlashCommandOption
        {
            option.IsRequired = isRequired;
            return option;
        }

        public static TApplicationCommandOption WithChoices<TApplicationCommandOption>(this TApplicationCommandOption option, IEnumerable<LocalSlashCommandOptionChoice> choices)
            where TApplicationCommandOption : LocalSlashCommandOption
        {
            if (choices == null)
                throw new ArgumentNullException(nameof(choices));

            option._choices.Clear();
            option._choices.AddRange(choices);
            return option;
        }

        public static TApplicationCommandOption WithOptions<TApplicationCommandOption>(this TApplicationCommandOption option, IEnumerable<LocalSlashCommandOption> options)
            where TApplicationCommandOption : LocalSlashCommandOption
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            option._options.Clear();
            option._options.AddRange(options);
            return option;
        }

        public static TApplicationCommandOption WithChannelTypes<TApplicationCommandOption>(this TApplicationCommandOption option, IEnumerable<ChannelType> channelTypes)
            where TApplicationCommandOption : LocalSlashCommandOption
        {
            if (channelTypes == null)
                throw new ArgumentNullException(nameof(channelTypes));

            option._channelTypes.Clear();
            option._channelTypes.AddRange(channelTypes);
            return option;
        }
    }
}
