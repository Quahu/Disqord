using System;
using System.Collections.Generic;

namespace Disqord
{
    public static class LocalSlashCommandOptionExtensions
    {
        public static TSlashCommandOption WithType<TSlashCommandOption>(this TSlashCommandOption option, SlashCommandOptionType type)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            option.Type = type;
            return option;
        }

        public static TSlashCommandOption WithName<TSlashCommandOption>(this TSlashCommandOption option, string name)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            option.Name = name;
            return option;
        }

        public static TSlashCommandOption WithDescription<TSlashCommandOption>(this TSlashCommandOption option, string description)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            option.Description = description;
            return option;
        }

        public static TSlashCommandOption WithIsRequired<TSlashCommandOption>(this TSlashCommandOption option, bool isRequired)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            option.IsRequired = isRequired;
            return option;
        }

        public static TSlashCommandOption WithChoices<TSlashCommandOption>(this TSlashCommandOption option, IEnumerable<LocalSlashCommandOptionChoice> choices)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            if (choices == null)
                throw new ArgumentNullException(nameof(choices));

            option._choices.Clear();
            option._choices.AddRange(choices);
            return option;
        }

        public static TSlashCommandOption WithOptions<TSlashCommandOption>(this TSlashCommandOption option, IEnumerable<LocalSlashCommandOption> options)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            option._options.Clear();
            option._options.AddRange(options);
            return option;
        }

        public static TSlashCommandOption WithChannelTypes<TSlashCommandOption>(this TSlashCommandOption option, IEnumerable<ChannelType> channelTypes)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            if (channelTypes == null)
                throw new ArgumentNullException(nameof(channelTypes));

            option._channelTypes.Clear();
            option._channelTypes.AddRange(channelTypes);
            return option;
        }
    }
}
