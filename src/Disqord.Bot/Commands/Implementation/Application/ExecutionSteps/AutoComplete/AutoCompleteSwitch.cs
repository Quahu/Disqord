using System.Threading.Tasks;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Application;

public static partial class DefaultApplicationExecutionSteps
{
    public class AutoCompleteSwitch : CommandExecutionStep
    {
        protected override ValueTask<IResult> OnExecuted(ICommandContext context)
        {
            Guard.IsNotNull(context.Command);
            var applicationCommand = Guard.IsAssignableToType<ApplicationCommand>(context.Command);
            var autoCompleteCommand = applicationCommand.AutoCompleteCommand;
            if (autoCompleteCommand == null)
                return Results.Failure("The command does not have auto-complete registered.");

            context.Command = autoCompleteCommand;
            return Next.ExecuteAsync(context);
        }
    }
}
