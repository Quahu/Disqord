using System;
using System.Collections.Generic;
using System.Linq;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord
{
    public static class LocalApplicationCommandOptionExtensions
    {
        public static TApplicationCommandOption WithType<TApplicationCommandOption>(this TApplicationCommandOption option, ApplicationCommandOptionType type)
            where TApplicationCommandOption : LocalApplicationCommandOption
        {
            option.Type = type;
            return option;
        }

        public static TApplicationCommandOption WithName<TApplicationCommandOption>(this TApplicationCommandOption option, string name)
            where TApplicationCommandOption : LocalApplicationCommandOption
        {
            option.Name = name;
            return option;
        }

        public static TApplicationCommandOption WithDescription<TApplicationCommandOption>(this TApplicationCommandOption option, string description)
            where TApplicationCommandOption : LocalApplicationCommandOption
        {
            option.Description = description;
            return option;
        }

        public static TApplicationCommandOption WithIsRequired<TApplicationCommandOption>(this TApplicationCommandOption option, bool isRequired)
            where TApplicationCommandOption : LocalApplicationCommandOption
        {
            option.IsRequired = isRequired;
            return option;
        }

        public static TApplicationCommandOption WithChoices<TApplicationCommandOption>(this TApplicationCommandOption option, IEnumerable<LocalApplicationCommandOptionChoice> choices)
            where TApplicationCommandOption : LocalApplicationCommandOption
        {
            if (choices == null)
                throw new ArgumentNullException(nameof(choices));

            option._choices.Clear();
            option._choices.AddRange(choices);
            return option;
        }

        public static TApplicationCommandOption WithOptions<TApplicationCommandOption>(this TApplicationCommandOption option, IEnumerable<LocalApplicationCommandOption> options)
            where TApplicationCommandOption : LocalApplicationCommandOption
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            option._options.Clear();
            option._options.AddRange(options);
            return option;
        }

        public static TApplicationCommandOption WithChannelTypes<TApplicationCommandOption>(this TApplicationCommandOption option, IEnumerable<ChannelType> channelTypes)
            where TApplicationCommandOption : LocalApplicationCommandOption
        {
            if (channelTypes == null)
                throw new ArgumentNullException(nameof(channelTypes));

            option._channelTypes.Clear();
            option._channelTypes.AddRange(channelTypes);
            return option;
        }

        public static ApplicationCommandOptionJsonModel ToModel(this LocalApplicationCommandOption option, IJsonSerializer serializer)
            => option == null ? null : new ApplicationCommandOptionJsonModel
            {
                Type = option.Type,
                Name = option.Name,
                Description = option.Description,
                Required = option.IsRequired,
                Choices = option.Choices.Select(x => x.ToModel(serializer)).ToArray(),
                Options = option.Options.Select(x => x.ToModel(serializer)).ToArray()
            };

        public static ApplicationCommandOptionChoiceJsonModel ToModel(this LocalApplicationCommandOptionChoice choice, IJsonSerializer serializer)
            => choice == null ? null : new ApplicationCommandOptionChoiceJsonModel
            {
                Name = choice.Name,
                Value = (IJsonValue)serializer.GetJsonNode(choice.Value)
            };
    }
}
