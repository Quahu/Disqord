using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Bot.Commands.Interaction;

public class DiscordInteractionDeferralCommandResult : DiscordCommandResult<IDiscordInteractionCommandContext>
{
    /// <summary>
    ///     Gets or sets whether the followups to this deferral should be ephemeral.
    /// </summary>
    public bool IsEphemeral { get; set; }

    public DiscordInteractionDeferralCommandResult(IDiscordInteractionCommandContext context, bool isEphemeral)
        : base(context)
    {
        IsEphemeral = isEphemeral;
    }

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return Context.Interaction.Response().DeferAsync(IsEphemeral, cancellationToken: cancellationToken);
    }
}
