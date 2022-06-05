using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Bot.Commands.Application.Default;

public partial class DefaultApplicationCommandCacheProvider
{
    public class CacheJsonModel : JsonModel
    {
        [JsonProperty("v")]
        public int SchemaVersion;

        [JsonProperty("g")]
        public CommandJsonModel[]? GlobalCommands = null!;

        [JsonProperty("s")]
        public Dictionary<Snowflake, CommandJsonModel[]>? GuildCommands = null!;
    }

    public class CommandJsonModel : JsonModel, IEquatable<LocalApplicationCommand>
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("t")]
        public ApplicationCommandType Type;

        [JsonProperty("n")]
        public string Name = null!;

        [JsonProperty("nl")]
        public Optional<Dictionary<string, string>> NameLocalizations;

        [JsonProperty("d")]
        public Optional<string> Description;

        [JsonProperty("dl")]
        public Optional<Dictionary<string, string>> DescriptionLocalizations;

        [JsonProperty("o")]
        public Optional<OptionJsonModel[]> Options;

        [JsonProperty("rmp")]
        public Optional<ulong> RequiredMemberPermissions;

        [JsonProperty("dm")]
        public Optional<bool> IsEnabledInPrivateChannels;

        public CommandJsonModel()
        { }

        public CommandJsonModel(LocalApplicationCommand command)
        {
            Populate(command);
        }

        public void Populate(LocalApplicationCommand command)
        {
            OptionalGuard.HasValue(command.Name);

            Type = GetCommandType(command);
            Name = command.Name.Value;
            NameLocalizations = Optional.Convert(command.NameLocalizations, localizations => localizations.ToDictionary(x => x.Key.Name, x => x.Value));

            if (command is LocalSlashCommand slashCommand)
            {
                OptionalGuard.HasValue(slashCommand.Description);

                Description = slashCommand.Description;
                DescriptionLocalizations = Optional.Convert(slashCommand.DescriptionLocalizations, localizations => localizations.ToDictionary(x => x.Key.Name, x => x.Value));
                Options = Optional.Convert(slashCommand.Options, options => options.Select(option => new OptionJsonModel(option)).ToArray());
            }

            RequiredMemberPermissions = Optional.Convert(command.DefaultRequiredMemberPermissions, permissions => (ulong) permissions);
            IsEnabledInPrivateChannels = command.IsEnabledInPrivateChannels;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ApplicationCommandType GetCommandType(LocalApplicationCommand command)
        {
            return command switch
            {
                LocalSlashCommand => ApplicationCommandType.Slash,
                LocalUserContextMenuCommand => ApplicationCommandType.User,
                LocalMessageContextMenuCommand => ApplicationCommandType.Message,
                _ => Throw.InvalidOperationException<ApplicationCommandType>("Invalid application command type.")
            };
        }

        /// <inheritdoc />
        public bool Equals(LocalApplicationCommand? command)
        {
            if (command == null)
                return false;

            if (Type != GetCommandType(command)
                || Name != command.Name
                || !AreLocalizationsEquivalent(NameLocalizations, command.NameLocalizations))
                return false;

            if (command is LocalSlashCommand slashCommand)
            {
                if (Description != slashCommand.Description
                    || !AreLocalizationsEquivalent(DescriptionLocalizations, slashCommand.DescriptionLocalizations)
                    || !AreEqual(Options, slashCommand.Options))
                    return false;
            }

            if (RequiredMemberPermissions != Optional.Convert(command.DefaultRequiredMemberPermissions, permissions => (ulong) permissions)
                || IsEnabledInPrivateChannels != command.IsEnabledInPrivateChannels)
                return false;

            return true;
        }
    }

    public class OptionJsonModel : JsonModel, IEquatable<LocalSlashCommandOption>
    {
        [JsonProperty("t")]
        public SlashCommandOptionType Type;

        [JsonProperty("n")]
        public string Name = null!;

        [JsonProperty("nl")]
        public Optional<Dictionary<string, string>> NameLocalizations;

        [JsonProperty("d")]
        public string Description = null!;

        [JsonProperty("dl")]
        public Optional<Dictionary<string, string>> DescriptionLocalizations;

        [JsonProperty("r")]
        public Optional<bool> IsRequired;

        [JsonProperty("c")]
        public Optional<ChoiceJsonModel[]> Choices;

        [JsonProperty("o")]
        public Optional<OptionJsonModel[]> Options;

        [JsonProperty("ct")]
        public Optional<ChannelType[]> ChannelTypes;

        [JsonProperty("mn")]
        public Optional<double> MinimumValue;

        [JsonProperty("mx")]
        public Optional<double> MaximumValue;

        [JsonProperty("ac")]
        public Optional<bool> HasAutoComplete;

        public OptionJsonModel()
        { }

        public OptionJsonModel(LocalSlashCommandOption option)
        {
            OptionalGuard.HasValue(option.Type);
            OptionalGuard.HasValue(option.Name);
            OptionalGuard.HasValue(option.Description);

            Type = option.Type.Value;
            Name = option.Name.Value;
            NameLocalizations = Optional.Convert(option.NameLocalizations, localizations => localizations.ToDictionary(x => x.Key.Name, x => x.Value));
            Description = option.Description.Value;
            DescriptionLocalizations = Optional.Convert(option.DescriptionLocalizations, localizations => localizations.ToDictionary(x => x.Key.Name, x => x.Value));
            IsRequired = option.IsRequired;
            Choices = Optional.Convert(option.Choices, choices => choices.Select(choice => new ChoiceJsonModel(choice)).ToArray());
            Options = Optional.Convert(option.Options, options => options.Select(option => new OptionJsonModel(option)).ToArray());
            ChannelTypes = Optional.Convert(option.ChannelTypes, channelTypes => channelTypes.ToArray());
            MinimumValue = option.MinimumValue;
            MaximumValue = option.MaximumValue;
            HasAutoComplete = option.HasAutoComplete;
        }

        /// <inheritdoc />
        public bool Equals(LocalSlashCommandOption? option)
        {
            if (option == null)
                return false;

            if (Type != option.Type
                || Name != option.Name
                || !AreLocalizationsEquivalent(NameLocalizations, option.NameLocalizations)
                || Description != option.Description
                || !AreLocalizationsEquivalent(DescriptionLocalizations, option.DescriptionLocalizations)
                || IsRequired != option.IsRequired
                || !AreEqual(Choices, option.Choices)
                || !AreEqual(Options, option.Options)
                || !AreEquivalent(ChannelTypes, option.ChannelTypes)
                || MinimumValue != option.MinimumValue
                || MaximumValue != option.MaximumValue
                || HasAutoComplete != option.HasAutoComplete)
                return false;

            return true;
        }
    }

    public class ChoiceJsonModel : JsonModel, IEquatable<LocalSlashCommandOptionChoice>
    {
        [JsonProperty("n")]
        public string Name = null!;

        [JsonProperty("nl")]
        public Optional<Dictionary<string, string>> NameLocalizations;

        [JsonProperty("v")]
        public object Value = null!;

        public ChoiceJsonModel()
        { }

        public ChoiceJsonModel(LocalSlashCommandOptionChoice choice)
        {
            OptionalGuard.HasValue(choice.Name);
            OptionalGuard.HasValue(choice.Value);

            Name = choice.Name.Value;
            NameLocalizations = Optional.Convert(choice.NameLocalizations, localizations => localizations.ToDictionary(x => x.Key.Name, x => x.Value));
            Value = choice.Value.Value;
        }

        private static bool AreValuesEqual(object x, object y)
        {
            var xConvertible = Guard.IsAssignableToType<IConvertible>(x);
            var yConvertible = Guard.IsAssignableToType<IConvertible>(y);
            var xTypeCode = xConvertible.GetTypeCode();
            var yTypeCode = yConvertible.GetTypeCode();
            if ((xTypeCode == TypeCode.String || yTypeCode == TypeCode.String) && xTypeCode != yTypeCode)
                return false;

            if (xTypeCode is TypeCode.String && yTypeCode is TypeCode.String)
                return (x as string)!.Equals(y as string);

            var xValue = xConvertible.ToDouble(null);
            var yValue = yConvertible.ToDouble(null);
            return xValue.Equals(yValue);
        }

        /// <inheritdoc />
        public bool Equals(LocalSlashCommandOptionChoice? choice)
        {
            if (choice == null)
                return false;

            if (Name != choice.Name
                || !AreLocalizationsEquivalent(NameLocalizations, choice.NameLocalizations)
                || !AreValuesEqual(Value, choice.Value.Value))
                return false;

            return true;
        }
    }
}
