using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Bot.Commands.Interaction;

public class DiscordInteractionDeferralCommandResult : DiscordCommandResult<IDiscordInteractionCommandContext>
{
    public bool IsEphemeral { get; }

    public DiscordInteractionDeferralCommandResult(IDiscordInteractionCommandContext context, bool isEphemeral)
        : base(context)
    {
        IsEphemeral = isEphemeral;
    }

    public override Task ExecuteAsync()
    {
        return Context.Interaction.Response().DeferAsync(IsEphemeral);
    }
}
