using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest;
using Microsoft.Extensions.Logging;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Application;

public static partial class DefaultApplicationExecutionSteps
{
    public class AutoCompleteResponse : CommandExecutionStep
    {
        protected override bool CanBeSkipped(ICommandContext context)
        {
            return context.Arguments == null || context.Arguments.Count == 0;
        }

        protected override async ValueTask<IResult> OnExecuted(ICommandContext context)
        {
            var result = await Next.ExecuteAsync(context).ConfigureAwait(false);
            if (!result.IsSuccessful)
                return result;

            var applicationContext = Guard.IsAssignableToType<IDiscordApplicationCommandContext>(context);
            var response = applicationContext.Interaction.Response();
            if (response.HasResponded)
                return result;

            foreach (var argument in applicationContext.Arguments!.Values)
            {
                Guard.IsNotNull(argument);
                if (argument is not IAutoComplete autoComplete || !autoComplete.IsFocused)
                    continue;

                var enumerator = autoComplete.EnumerateChoices();
                var choices = new List<KeyValuePair<string, object>>();
                while (enumerator.MoveNext())
                {
                    choices.Add(KeyValuePair.Create(
                        (enumerator.Key as string)!,
                        enumerator.Value!
                    ));
                }

                try
                {
                    await response.AutoCompleteAsync(choices).ConfigureAwait(false);
                }
                catch (Exception ex) when (ex is InteractionExpiredException || ex is RestApiException restApiException && restApiException.IsError(RestApiErrorCode.UnknownInteraction))
                {
                    applicationContext.Bot.Logger.LogWarning("Auto-complete handler '{0}' failed to respond to the interaction in time.", context.Command!.Name);
                }

                break;
            }

            return Results.Success;
        }
    }
}
