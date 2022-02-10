using System.Collections.Generic;
using Qommon;

namespace Disqord
{
    public static class LocalSlashCommandOptionExtensions
    {
        public static TSlashCommandOption WithType<TSlashCommandOption>(this TSlashCommandOption @this, SlashCommandOptionType type)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            @this.Type = type;
            return @this;
        }

        public static TSlashCommandOption WithName<TSlashCommandOption>(this TSlashCommandOption @this, string name)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            @this.Name = name;
            return @this;
        }

        public static TSlashCommandOption WithDescription<TSlashCommandOption>(this TSlashCommandOption @this, string description)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            @this.Description = description;
            return @this;
        }

        public static TSlashCommandOption WithIsRequired<TSlashCommandOption>(this TSlashCommandOption @this, bool isRequired = true)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            @this.IsRequired = isRequired;
            return @this;
        }

        public static TSlashCommandOption AddChoice<TSlashCommandOption>(this TSlashCommandOption @this, LocalSlashCommandOptionChoice choice)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            Guard.IsNotNull(choice);

            if (!@this.Choices.Add(choice, out var list))
                @this.Choices = new(list);

            return @this;
        }

        public static TSlashCommandOption WithChoices<TSlashCommandOption>(this TSlashCommandOption @this, IEnumerable<LocalSlashCommandOptionChoice> choices)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            Guard.IsNotNull(choices);

            if (!@this.Choices.With(choices, out var list))
                @this.Choices = new(list);

            return @this;
        }

        public static TSlashCommandOption WithChoices<TSlashCommandOption>(this TSlashCommandOption @this, params LocalSlashCommandOptionChoice[] choices)
            where TSlashCommandOption : LocalSlashCommandOption
            => @this.WithChoices(choices as IEnumerable<LocalSlashCommandOptionChoice>);

        public static TSlashCommandOption WithHasAutoComplete<TSlashCommandOption>(this TSlashCommandOption @this, bool hasAutoComplete = true)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            @this.HasAutoComplete = hasAutoComplete;
            return @this;
        }

        public static TSlashCommandOption AddOption<TSlashCommandOption>(this TSlashCommandOption @this, LocalSlashCommandOption option)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            Guard.IsNotNull(option);

            if (!@this.Options.Add(option, out var list))
                @this.Options = new(list);

            return @this;
        }

        public static TSlashCommandOption WithOptions<TSlashCommandOption>(this TSlashCommandOption @this, IEnumerable<LocalSlashCommandOption> options)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            Guard.IsNotNull(options);

            if (!@this.Options.With(options, out var list))
                @this.Options = new(list);

            return @this;
        }

        public static TSlashCommandOption WithOptions<TSlashCommandOption>(this TSlashCommandOption @this, params LocalSlashCommandOption[] options)
            where TSlashCommandOption : LocalSlashCommandOption
            => @this.WithOptions(options as IEnumerable<LocalSlashCommandOption>);

        public static TSlashCommandOption AddChannelType<TSlashCommandOption>(this TSlashCommandOption @this, ChannelType channelType)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            Guard.IsDefined(channelType);

            if (!@this.ChannelTypes.Add(channelType, out var list))
                @this.ChannelTypes = new(list);

            return @this;
        }

        public static TSlashCommandOption WithChannelTypes<TSlashCommandOption>(this TSlashCommandOption @this, IEnumerable<ChannelType> channelTypes)
            where TSlashCommandOption : LocalSlashCommandOption
        {
            Guard.IsNotNull(channelTypes);

            if (!@this.ChannelTypes.With(channelTypes, out var list))
                @this.ChannelTypes = new(list);

            return @this;
        }

        public static TSlashCommandOption WithChannelTypes<TSlashCommandOption>(this TSlashCommandOption @this, params ChannelType[] channelTypes)
            where TSlashCommandOption : LocalSlashCommandOption
            => @this.WithChannelTypes(channelTypes as IEnumerable<ChannelType>);
    }
}
