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
        protected override bool CanBeSkipped(ICommandContext context)
        {
            return context.Arguments == null || context.Arguments.Count == 0;
        }

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
                foreach (var kvp in context.Arguments!)
                {
                    if (parameter.Name != kvp.Key.Name)
                        continue;

                    originalKvp = kvp;
                    break;
                }

                object? currentValue;
                bool isFocused;
                if (originalKvp == null)
                {
                    currentValue = Activator.CreateInstance(typeof(Optional<>).MakeGenericType(parameter.ReflectedType.GenericTypeArguments[0]));
                    isFocused = false;
                }
                else
                {
                    currentValue = originalKvp.Value.Value is IOptional optional
                        ? optional
                        : Activator.CreateInstance(typeof(Optional<>).MakeGenericType(parameter.ReflectedType.GenericTypeArguments[0]), originalKvp.Value.Value);

                    isFocused = focusedOption == options[parameter.Name];
                }

                arguments[parameter] = Activator.CreateInstance(typeof(AutoComplete<>).MakeGenericType(parameter.ReflectedType.GenericTypeArguments[0]), currentValue, isFocused);
            }

            context.Arguments = arguments;
            return Next.ExecuteAsync(context);
        }
    }
}
