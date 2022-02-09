using System.Linq;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    internal static partial class ContentConversion
    {
        public static CreateApplicationCommandJsonRestRequestContent ToContent(this LocalApplicationCommand command, IJsonSerializer serializer)
        {
            Guard.IsNotNull(command);

            var content = new CreateApplicationCommandJsonRestRequestContent
            {
                Name = command.Name.Value,
                DefaultPermission = command.IsEnabledByDefault
            };

            if (command is LocalSlashCommand slashCommand)
            {
                content.Type = ApplicationCommandType.Slash;
                content.Description = slashCommand.Description;
                content.Options = Optional.Convert(slashCommand.Options, options => options?.Select(option => option?.ToModel(serializer)).ToArray());
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
                Description = option.Description.Value,
                Required = option.IsRequired,
                Choices = Optional.Convert(option.Choices, choices => choices?.Select(choice => choice?.ToModel(serializer)).ToArray()),
                AutoComplete = option.HasAutoComplete,
                Options = Optional.Convert(option.Options, options => options?.Select(option => option?.ToModel(serializer)).ToArray()),
                ChannelTypes = Optional.Convert(option.ChannelTypes, channelTypes => channelTypes?.ToArray())
            };
        }

        public static ApplicationCommandOptionChoiceJsonModel ToModel(this LocalSlashCommandOptionChoice choice, IJsonSerializer serializer)
        {
            Guard.IsNotNull(choice);
            OptionalGuard.HasValue(choice.Name);

            return new ApplicationCommandOptionChoiceJsonModel
            {
                Name = choice.Name.Value,
                Value = serializer.GetJsonNode(choice.Value.GetValueOrDefault()) as IJsonValue
            };
        }
    }
}
