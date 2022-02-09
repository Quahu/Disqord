using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class ApplicationCommandOptionJsonModel : JsonModel
    {
        [JsonProperty("type")]
        public SlashCommandOptionType Type;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("required")]
        public Optional<bool> Required;

        [JsonProperty("choices")]
        public Optional<ApplicationCommandOptionChoiceJsonModel[]> Choices;

        [JsonProperty("autocomplete")]
        public Optional<bool> AutoComplete;

        [JsonProperty("options")]
        public Optional<ApplicationCommandOptionJsonModel[]> Options;

        [JsonProperty("channel_types")]
        public Optional<ChannelType[]> ChannelTypes;

        protected override void OnValidate()
        {
            Guard.IsDefined(Type);

            Guard.IsNotNullOrWhiteSpace(Name);
            Guard.HasSizeBetweenOrEqualTo(Name, Discord.Limits.ApplicationCommands.Options.MinNameLength, Discord.Limits.ApplicationCommands.Options.MaxNameLength);

            Guard.IsNotNullOrWhiteSpace(Description);
            Guard.HasSizeBetweenOrEqualTo(Description, Discord.Limits.ApplicationCommands.Options.MinDescriptionLength, Discord.Limits.ApplicationCommands.Options.MaxDescriptionLength);

            if (Type is not SlashCommandOptionType.String and not SlashCommandOptionType.Integer and not SlashCommandOptionType.Number)
            {
                OptionalGuard.HasNoValue(Choices, "Choices can only be specified for string, integer, and number options.");

                if (AutoComplete.HasValue)
                    Guard.IsNotEqualTo(true, AutoComplete.Value);
            }

            if (Type is not SlashCommandOptionType.Subcommand and not SlashCommandOptionType.SubcommandGroup)
                OptionalGuard.HasNoValue(Options, "Nested options can only be specified for subcommands and subcommand groups.");

            if (AutoComplete.HasValue && AutoComplete.Value)
                OptionalGuard.HasNoValue(Choices, "Choices cannot be present when auto-complete is enabled.");

            OptionalGuard.CheckValue(Choices, value =>
            {
                Guard.IsNotNull(value);
                Guard.HasSizeLessThanOrEqualTo(value, Discord.Limits.ApplicationCommands.Options.MaxChoiceAmount);

                foreach (var choice in value)
                {
                    Guard.IsNotNull(choice);
                    choice.Validate();
                }
            });

            OptionalGuard.CheckValue(Options, value =>
            {
                Guard.IsNotNull(value);
                Guard.HasSizeLessThanOrEqualTo(value, Discord.Limits.ApplicationCommands.MaxOptionsAmount);

                foreach (var option in value)
                {
                    Guard.IsNotNull(option);
                    option.Validate();
                }
            });

            OptionalGuard.CheckValue(ChannelTypes, value =>
            {
                Guard.IsNotNull(value);

                foreach (var channelType in value)
                    Guard.IsDefined(channelType);
            });
        }
    }
}
