using System;
using System.Threading.Tasks;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands;

public static partial class DefaultBotExecutionSteps
{
    /// <summary>
    ///     Invokes <see cref="DiscordBotBase.OnBeforeExecuted"/>
    /// </summary>
    public class InvokeOnBeforeExecuted : CommandExecutionStep
    {
        /// <inheritdoc/>
        protected override async ValueTask<IResult> OnExecuted(ICommandContext context)
        {
            var discordContext = Guard.IsAssignableToType<IDiscordCommandContext>(context);

            try
            {
                var result = await discordContext.Bot.OnBeforeExecuted(discordContext).ConfigureAwait(false);
                if (!result.IsSuccessful)
                    return result;
            }
            catch (Exception ex)
            {
                return Results.Exception($"invoking {nameof(DiscordBotBase)}.{nameof(DiscordBotBase.OnBeforeExecuted)}", ex);
            }

            return await Next.ExecuteAsync(context).ConfigureAwait(false);
        }
    }
}
