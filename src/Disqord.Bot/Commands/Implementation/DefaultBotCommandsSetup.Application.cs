using Disqord.Bot.Commands.Application;
using Qmmands;
using Qmmands.Default;
using static Qmmands.Default.DefaultExecutionSteps;
using static Disqord.Bot.Commands.Application.DefaultApplicationExecutionSteps;
using static Disqord.Bot.Commands.DefaultBotExecutionSteps;

namespace Disqord.Bot.Commands;

public static partial class DefaultBotCommandsSetup
{
    public class ApplicationCommandPipeline : DefaultCommandPipeline<IDiscordApplicationCommandContext>
    {
        public ApplicationCommandPipeline()
        {
            this.Use<MapLookup>()
                .Use<RunChecks>()
                .Use<BindOptions>()
                .Use<TypeParse>()
                .Use<BindArguments>()
                .Use<RunParameterChecks>()
                .Use<ValidateArguments>()
                .Use<RunRateLimits>()
                .Use<CreateModuleBase>()
                .Use<InvokeOnBeforeExecuted>()
                .Use<ExecuteCommand>();
        }

        public override bool CanExecute(ICommandContext context)
        {
            return context is IDiscordApplicationCommandContext applicationCommandContext && applicationCommandContext.Interaction is IContextMenuInteraction or ISlashCommandInteraction;
        }
    }

    public class AutoCompleteCommandPipeline : DefaultCommandPipeline<IDiscordApplicationCommandContext>
    {
        public AutoCompleteCommandPipeline()
        {
            this.Use<MapLookup>()
                .Use<RunChecks>()
                .Use<BindOptions>()
                .Use<TypeParse>()
                .Use<AutoCompleteSwitch>()
                .Use<BindAutoComplete>()
                .Use<BindArguments>()
                .Use<RunParameterChecks>()
                .Use<ValidateArguments>()
                .Use<RunRateLimits>()
                .Use<AutoCompleteResponse>()
                .Use<CreateModuleBase>()
                .Use<InvokeOnBeforeExecuted>()
                .Use<ExecuteCommand>();
        }

        public override bool CanExecute(ICommandContext context)
        {
            return context is IDiscordApplicationCommandContext applicationCommandContext && applicationCommandContext.Interaction is IAutoCompleteInteraction;
        }
    }
}
