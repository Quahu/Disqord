using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Components;

public static partial class DefaultComponentExecutionSteps
{
    public class MapLookup : CommandExecutionStep
    {
        protected override bool CanBeSkipped(ICommandContext context)
        {
            return context.Command != null;
        }

        protected override ValueTask<IResult> OnExecuted(ICommandContext context)
        {
            var componentContext = Guard.IsAssignableToType<IDiscordComponentCommandContext>(context);
            var map = context.Services.GetRequiredService<ICommandMapProvider>().GetRequiredMap<ComponentCommandMap>();
            var command = map.FindCommand(componentContext.Interaction, out var rawArguments);
            if (command == null)
                return new(CommandNotFoundResult.Instance);

            context.Command = command;
            if (rawArguments != null)
            {
                Dictionary<IParameter, MultiString>? dictionary = null;
                var parameters = command.Parameters;
                var parameterCount = parameters.Count;
                var index = -1;
                foreach (var rawArgument in rawArguments)
                {
                    index++;
                    if (index == parameterCount)
                        break;

                    var parameter = parameters[index];
                    (dictionary ??= new(4))[parameter] = rawArgument;
                }

                context.RawArguments = dictionary;
            }

            return Next.ExecuteAsync(context);
        }
    }
}
