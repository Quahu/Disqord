using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Bot.Commands.Interaction;

/// <summary>
///     Represents <see cref="IDiscordInteractionCommandContext"/> extensions.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class DiscordInteractionCommandContextExtensions
{
    internal static Task SendMessageAsync(this IDiscordInteractionCommandContext context, LocalInteractionMessageResponse message,
        bool fetchMessage,
        IRestRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var interaction = context.Interaction;
        var response = interaction.Response();
        if (!response.HasResponded)
        {
            static async Task<IUserMessage> SendMessageWithResult(InteractionResponseHelper response, LocalInteractionMessageResponse message,
                IRestRequestOptions? options, CancellationToken cancellationToken)
            {
                await response.SendMessageAsync(message, options, cancellationToken).ConfigureAwait(false);
                return await response.Interaction.Followup().FetchResponseAsync(options, cancellationToken).ConfigureAwait(false);
            }

            return fetchMessage
                ? SendMessageWithResult(response, message, options, cancellationToken)
                : response.SendMessageAsync(message, options, cancellationToken);
        }

        var followup = interaction.Followup();
        return followup.SendAsync(message, options, cancellationToken);
    }
}
