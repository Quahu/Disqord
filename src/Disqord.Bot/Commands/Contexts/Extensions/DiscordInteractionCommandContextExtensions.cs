using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Bot.Commands.Interaction;

/// <summary>
///     Represents <see cref="IDiscordInteractionCommandContext"/> extensions.
/// </summary>
internal static class DiscordInteractionCommandContextExtensions
{
    internal static Task SendMessageAsync(this IDiscordInteractionCommandContext context, LocalInteractionMessageResponse message,
        IRestRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var interaction = context.Interaction;
        var response = interaction.Response();
        if (!response.HasResponded)
        {
            return response.SendMessageAsync(message, options, cancellationToken);
        }

        var followup = interaction.Followup();
        return followup.SendAsync(message, options, cancellationToken);
    }
}
