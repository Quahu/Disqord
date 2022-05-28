using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway;
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

                    if (parameter.ReflectedType.IsInstanceOfType(option.Value))
                    {
                        arguments[parameter] = option.Value;
                        continue;
                    }

                    if (option.Value is not string stringValue)
                    {
                        arguments[parameter] = Convert.ChangeType(option.Value, parameter.ReflectedType, context.Locale);
                        continue;
                    }

                    if (!option.Type.IsEntity())
                    {
                        // If the option is just a string, pass it through to type parsing.
                        (context.RawArguments ??= new Dictionary<IParameter, MultiString>())[parameter] = option.Value as string;
                        continue;
                    }

                    // If the option is an entity, parse the string as a snowflake and resolve the entity.
                    static ISnowflakeEntity? GetChannel(IApplicationCommandInteraction interaction, Snowflake channelId)
                    {
                        IChannel? channel = null;
                        if (interaction.GuildId != null)
                            channel = (interaction.Client as DiscordClientBase)!.GetChannel(interaction.GuildId.Value, channelId);

                        return channel ?? interaction.Entities.Channels[channelId];
                    }

                    var entityId = Snowflake.Parse(stringValue);
                    var entities = interaction.Entities;
                    var entity = option.Type switch
                    {
                        // TODO: do something about partial channels
                        SlashCommandOptionType.User => entities.Users[entityId],
                        SlashCommandOptionType.Channel => GetChannel(interaction, entityId),
                        SlashCommandOptionType.Role => entities.Roles[entityId],
                        SlashCommandOptionType.Attachment => entities.Attachments[entityId],
                        SlashCommandOptionType.Mentionable => entities.Users.GetValueOrDefault(entityId)
                            ?? entities.Roles.GetValueOrDefault(entityId) ?? GetChannel(interaction, entityId),
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
