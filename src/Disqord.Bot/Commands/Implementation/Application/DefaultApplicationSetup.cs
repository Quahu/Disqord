using Disqord.Bot.Commands.Application;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;
using Qmmands.Default;
using static Qmmands.Default.DefaultExecutionSteps;
using static Disqord.Bot.Commands.Application.DefaultApplicationExecutionSteps;

namespace Disqord.Bot.Commands;

public static class DefaultApplicationSetup
{
    public static void Initialize(ICommandService commands)
    {
        var services = commands.Services;
        var reflectorProvider = services.GetRequiredService<ICommandReflectorProvider>() as DefaultCommandReflectorProvider;
        reflectorProvider?.AddReflector(ActivatorUtilities.CreateInstance<ApplicationCommandReflector>(services));

        var pipelineProvider = services.GetRequiredService<ICommandPipelineProvider>() as DefaultCommandPipelineProvider;
        pipelineProvider?.Add(new AutoCompleteCommandPipeline());
        pipelineProvider?.Add(new ApplicationCommandPipeline());

        var mapProvider = services.GetRequiredService<ICommandMapProvider>() as DefaultCommandMapProvider;
        mapProvider?.Add(ActivatorUtilities.CreateInstance<ApplicationCommandMap>(services));
    }

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
                .Use<RunRateLimits>()
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
                .Use<RunRateLimits>()
                .Use<AutoCompleteResponse>()
                .Use<ExecuteCommand>();
        }

        public override bool CanExecute(ICommandContext context)
        {
            return context is IDiscordApplicationCommandContext applicationCommandContext && applicationCommandContext.Interaction is IAutoCompleteInteraction;
        }
    }
}
