using System.Linq;
using Disqord.Bot.Commands.Application;
using Disqord.Bot.Commands.Components;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;
using Qmmands.Default;
using Qmmands.Text;
using Qmmands.Text.Default;
using static Disqord.Bot.Commands.DefaultBotExecutionSteps;
using static Qmmands.Default.DefaultExecutionSteps;

namespace Disqord.Bot.Commands;

public static partial class DefaultBotCommandsSetup
{
    public static void Initialize(ICommandService commands)
    {
        DefaultTextSetup.Initialize(commands);

        var services = commands.Services;
        var reflectorProvider = services.GetRequiredService<ICommandReflectorProvider>() as DefaultCommandReflectorProvider;
        reflectorProvider?.AddReflector(ActivatorUtilities.CreateInstance<ApplicationCommandReflector>(services));
        reflectorProvider?.AddReflector(ActivatorUtilities.CreateInstance<ComponentCommandReflector>(services));

        var pipelineProvider = services.GetRequiredService<ICommandPipelineProvider>() as DefaultCommandPipelineProvider;
        var textPipeline = pipelineProvider?.ElementAtOrDefault(0) as DefaultCommandPipeline<ITextCommandContext>;
        if (textPipeline != null)
        {
            // We inject InvokeOnBeforeExecuted into the text pipeline...
            var textPipelineCount = textPipeline.Count;
            for (var i = 0; i < textPipelineCount; i++)
            {
                if (textPipeline[i] is not ExecuteCommand)
                    continue;

                // ...right before the ExecuteCommand step.
                textPipeline.Insert(i, new InvokeOnBeforeExecuted());
                break;
            }
        }

        pipelineProvider?.Add(new AutoCompleteCommandPipeline());
        pipelineProvider?.Add(new ApplicationCommandPipeline());
        pipelineProvider?.Add(new ComponentCommandPipeline());

        var mapProvider = services.GetRequiredService<ICommandMapProvider>() as DefaultCommandMapProvider;
        mapProvider?.Add(ActivatorUtilities.CreateInstance<ApplicationCommandMap>(services));
        mapProvider?.Add(ActivatorUtilities.CreateInstance<ComponentCommandMap>(services));
    }
}
