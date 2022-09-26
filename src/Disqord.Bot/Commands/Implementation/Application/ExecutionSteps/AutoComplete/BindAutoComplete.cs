using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Qmmands;
using Qommon;
using Qommon.Serialization;

namespace Disqord.Bot.Commands.Application;

public static partial class DefaultApplicationExecutionSteps
{
    public class BindAutoComplete : CommandExecutionStep
    {
        protected override ValueTask<IResult> OnExecuted(ICommandContext context)
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

            var interaction = ((context as IDiscordApplicationCommandContext)!.Interaction as IAutoCompleteInteraction)!;
            var focusedOption = interaction.FocusedOption;
            var options = GetArguments(interaction.Options);
            var arguments = new Dictionary<IParameter, object?>();
            foreach (var parameter in context.Command!.Parameters)
            {
                KeyValuePair<IParameter, object?>? originalKvp = null;
                if (context.Arguments != null)
                {
                    foreach (var kvp in context.Arguments)
                    {
                        if (parameter.Name != kvp.Key.Name)
                            continue;

                        originalKvp = kvp;
                        break;
                    }
                }

                var isFocused = focusedOption == options.GetValueOrDefault(parameter.Name);
                var parameterType = parameter.ReflectedType.GenericTypeArguments[0];
                object currentValue;
                if (originalKvp == null)
                {
                    currentValue = Activator.CreateInstance(typeof(Optional<>).MakeGenericType(parameterType))!;
                }
                else
                {
                    currentValue = isFocused || originalKvp.Value.Value is IOptional
                        ? originalKvp.Value.Value!
                        : Activator.CreateInstance(typeof(Optional<>).MakeGenericType(parameterType), originalKvp.Value.Value)!;
                }

                if (typeof(IAutoComplete).IsAssignableFrom(parameter.ReflectedType))
                {
                    var autoCompleteType = isFocused ? typeof(FocusedAutoComplete<>) : typeof(UnfocusedAutoComplete<>);
                    var autoCompleteGenericType = autoCompleteType.MakeGenericType(parameterType);
                    currentValue = Activator.CreateInstance(autoCompleteGenericType, currentValue)!;
                }

                arguments[parameter] = currentValue;
            }

            context.Arguments = arguments;
            return Next.ExecuteAsync(context);
        }
    }
}
