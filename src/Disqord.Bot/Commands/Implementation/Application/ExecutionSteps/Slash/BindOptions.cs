using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Application;

public static partial class DefaultApplicationExecutionSteps
{
    public class BindOptions : CommandExecutionStep
    {
        protected override bool CanBeSkipped(ICommandContext context)
        {
            if (context is IDiscordApplicationCommandContext applicationContext)
            {
                if (applicationContext.Interaction is ISlashCommandInteraction slashInteraction)
                {
                    return slashInteraction.Options.Count == 0;
                }

                if (applicationContext.Interaction is IAutoCompleteInteraction autoCompleteInteraction)
                {
                    return autoCompleteInteraction.Options.Count == 0;
                }
            }

            return false;
        }

        protected override ValueTask<IResult> OnExecuted(ICommandContext context)
        {
            Guard.IsNotNull(context.Command);

            var applicationContext = Guard.IsAssignableToType<IDiscordApplicationCommandContext>(context);
            var interaction = applicationContext.Interaction;
            if (interaction is IContextMenuInteraction contextMenuInteraction)
            {
                var entityId = contextMenuInteraction.TargetId;
                var arguments = context.Arguments ??= new Dictionary<IParameter, object?>();
                var parameter = context.Command.Parameters[0];
                arguments[parameter] = contextMenuInteraction.CommandType switch
                {
                    ApplicationCommandType.User => contextMenuInteraction.Entities.Users[entityId],
                    ApplicationCommandType.Message => contextMenuInteraction.Entities.Messages[entityId],
                    _ => Throw.InvalidOperationException<object>($"Unsupported context menu command type: {contextMenuInteraction.CommandType}.")
                };
            }
            else if (interaction is ISlashCommandInteraction or IAutoCompleteInteraction)
            {
                static IReadOnlyDictionary<string, ISlashCommandInteractionOption> GetArguments(IReadOnlyDictionary<string, ISlashCommandInteractionOption> options)
                {
                    foreach (var (_, option) in options)
                    {
                        if (option.Type is SlashCommandOptionType.SubcommandGroup or SlashCommandOptionType.Subcommand)
                            return GetArguments(option.Options);

                        break;
                    }

                    return options;
                }

                var options = GetArguments((interaction as ISlashCommandInteraction)?.Options ?? (interaction as IAutoCompleteInteraction)!.Options);
                var parameters = context.Command.Parameters;
                var parameterCount = context.Command.Parameters.Count;
                for (var i = 0; i < parameterCount; i++)
                {
                    var parameter = parameters[i];
                    if (!options.TryGetValue(parameter.Name, out var option))
                        continue;

                    var arguments = context.Arguments ??= new Dictionary<IParameter, object?>();
                    if (option.Value == null)
                    {
                        arguments[parameter] = null;
                        continue;
                    }

                    var actualType = parameter.GetTypeInformation().ActualType;
                    if (actualType.IsInstanceOfType(option.Value))
                    {
                        arguments[parameter] = option.Value;
                        continue;
                    }

                    if (option.Value is not string stringValue)
                    {
                        arguments[parameter] = actualType.IsEnum
                            ? Enum.ToObject(actualType, option.Value)
                            : Convert.ChangeType(option.Value, actualType, context.Locale);

                        continue;
                    }

                    if (interaction is IAutoCompleteInteraction)
                    {
                        // Treat string values as arguments for auto-complete.
                        // See: https://github.com/discord/discord-api-docs/issues/4956
                        arguments[parameter] = stringValue;
                        continue;
                    }

                    if (option.Type == SlashCommandOptionType.String)
                    {
                        // If the option is just a string, pass it through to type parsing.
                        var rawArguments = context.RawArguments ??= new Dictionary<IParameter, MultiString>();
                        rawArguments[parameter] = option.Value as string;
                        continue;
                    }

                    // If the option is an entity, parse the string as a snowflake and resolve the entity.
                    var entityId = Snowflake.Parse(stringValue);
                    var entities = interaction.Entities;
                    var entity = option.Type switch
                    {
                        SlashCommandOptionType.User => entities.Users[entityId],
                        SlashCommandOptionType.Channel => interaction.Entities.Channels[entityId],
                        SlashCommandOptionType.Role => entities.Roles[entityId],
                        SlashCommandOptionType.Attachment => entities.Attachments[entityId],
                        SlashCommandOptionType.Mentionable => entities.Users.GetValueOrDefault(entityId)
                            ?? entities.Roles.GetValueOrDefault(entityId)
                            ?? entities.Channels.GetValueOrDefault(entityId) as object,
                        _ => Throw.InvalidOperationException<object>("Unsupported entity slash command option type.")
                    };

                    arguments[parameter] = entity;
                }
            }
            else
            {
                Throw.InvalidOperationException("Unsupported interaction type.");
            }

            return Next.ExecuteAsync(context);
        }
    }
}
