using Disqord.Bot.Commands.Components;
using Qmmands;
using Qmmands.Default;
using static Disqord.Bot.Commands.Components.DefaultComponentExecutionSteps;
using static Disqord.Bot.Commands.DefaultBotExecutionSteps;
using static Qmmands.Default.DefaultExecutionSteps;

namespace Disqord.Bot.Commands;

public static partial class DefaultBotCommandsSetup
{
    public class ComponentCommandPipeline : DefaultCommandPipeline<IDiscordComponentCommandContext>
    {
        public ComponentCommandPipeline()
        {
            this.Use<MapLookup>()
                .Use<RunChecks>()
                .Use<BindValues>()
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
            return context is IDiscordComponentCommandContext componentCommandContext && componentCommandContext.Interaction is IComponentInteraction or IModalSubmitInteraction;
        }
    }
}
