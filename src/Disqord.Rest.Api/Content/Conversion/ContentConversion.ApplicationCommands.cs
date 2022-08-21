using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

internal static partial class ContentConversion
{
    // TODO: this should be be made separate for commands, options, and choices so that it validates the name/description constraints
    private static Optional<Dictionary<string, string>?> ConvertLocalizations(Optional<IDictionary<CultureInfo, string>> localizations,
        [CallerArgumentExpression("localizations")] string? localizationsExpression = null)
    {
        if (!localizations.HasValue)
            return default;

        Guard.IsNotNull(localizations.Value, localizationsExpression);
        var dictionary = new Dictionary<string, string>();
        foreach (var localization in localizations.Value)
        {
            Guard.IsNotNull(localization.Value);

            dictionary.Add(localization.Key.Name, localization.Value);
        }

        return dictionary;
    }

    public static CreateApplicationCommandJsonRestRequestContent ToContent(this LocalApplicationCommand command, IJsonSerializer serializer)
    {
        Guard.IsNotNull(command);
        OptionalGuard.HasValue(command.Name);

        var content = new CreateApplicationCommandJsonRestRequestContent
        {
            Name = command.Name.Value,
            NameLocalizations = ConvertLocalizations(command.NameLocalizations),
            DefaultMemberPermissions = Optional.Convert(command.DefaultRequiredMemberPermissions, defaultMemberPermissions => (Permissions?) defaultMemberPermissions),
            DmPermission = Optional.Convert(command.IsEnabledInPrivateChannels, isEnabledInPrivateChannels => (bool?) isEnabledInPrivateChannels),
            DefaultPermission = command.IsEnabledByDefault
        };

        if (command is LocalSlashCommand slashCommand)
        {
            OptionalGuard.HasValue(slashCommand.Description);

            content.Type = ApplicationCommandType.Slash;
            content.Description = slashCommand.Description;
            content.DescriptionLocalizations = ConvertLocalizations(slashCommand.DescriptionLocalizations);
            content.Options = Optional.Convert(slashCommand.Options, options => options?.Select(option => option?.ToModel(serializer)).ToArray())!;
        }
        else if (command is LocalUserContextMenuCommand)
        {
            content.Type = ApplicationCommandType.User;
        }
        else if (command is LocalMessageContextMenuCommand)
        {
            content.Type = ApplicationCommandType.Message;
        }

        return content;
    }

    public static ApplicationCommandOptionJsonModel ToModel(this LocalSlashCommandOption option, IJsonSerializer serializer)
    {
        Guard.IsNotNull(option);
        OptionalGuard.HasValue(option.Type);
        OptionalGuard.HasValue(option.Name);
        OptionalGuard.HasValue(option.Description);

        return new ApplicationCommandOptionJsonModel
        {
            Type = option.Type.Value,
            Name = option.Name.Value,
            NameLocalizations = ConvertLocalizations(option.NameLocalizations),
            Description = option.Description.Value,
            DescriptionLocalizations = ConvertLocalizations(option.DescriptionLocalizations),
            Required = option.IsRequired,
            Choices = Optional.Convert(option.Choices, choices => choices?.Select(choice => choice?.ToModel(serializer)).ToArray())!,
            Options = Optional.Convert(option.Options, options => options?.Select(option => option?.ToModel(serializer)).ToArray())!,
            ChannelTypes = Optional.Convert(option.ChannelTypes, channelTypes => channelTypes?.ToArray())!,
            MinValue = option.MinimumValue,
            MaxValue = option.MaximumValue,
            MinLength = option.MinimumLength,
            MaxLength = option.MaximumLength,
            AutoComplete = option.HasAutoComplete,
        };
    }

    public static ApplicationCommandOptionChoiceJsonModel ToModel(this LocalSlashCommandOptionChoice choice, IJsonSerializer serializer)
    {
        Guard.IsNotNull(choice);
        OptionalGuard.HasValue(choice.Name);

        return new ApplicationCommandOptionChoiceJsonModel
        {
            Name = choice.Name.Value,
            NameLocalizations = ConvertLocalizations(choice.NameLocalizations),
            Value = (serializer.GetJsonNode(choice.Value.GetValueOrDefault()) as IJsonValue)!
        };
    }
}
