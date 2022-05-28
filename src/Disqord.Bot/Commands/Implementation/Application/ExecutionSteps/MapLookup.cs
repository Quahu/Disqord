using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Application;

public static partial class DefaultApplicationExecutionSteps
{
    public class MapLookup : CommandExecutionStep
    {
        protected override bool CanBeSkipped(ICommandContext context)
        {
            return context.Command != null;
        }

        protected override ValueTask<IResult> OnExecuted(ICommandContext context)
        {
            var applicationContext = Guard.IsAssignableToType<IDiscordApplicationCommandContext>(context);
            var map = context.Services.GetRequiredService<ICommandMapProvider>().GetRequiredMap<ApplicationCommandMap>();
            var command = map.FindCommand(applicationContext.Interaction);
            if (command == null)
                return new(CommandNotFoundResult.Instance);

            context.Command = command;
            return Next.ExecuteAsync(context);
        }
    }
}
