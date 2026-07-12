using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest;
using Qommon;

namespace Disqord.Bot.Commands.Interaction;

/// <summary>
///     Represents a command result that responds to the interaction with a modal.
/// </summary>
public class DiscordInteractionModalCommandResult : DiscordCommandResult<IDiscordInteractionCommandContext>
{
    /// <summary>
    ///     Gets the modal to respond with.
    /// </summary>
    public LocalInteractionModalResponse Modal { get; }

    /// <summary>
    ///     Instantiates a new <see cref="DiscordInteractionModalCommandResult"/>.
    /// </summary>
    /// <param name="context"> The command context. </param>
    /// <param name="modal"> The modal to respond with. </param>
    public DiscordInteractionModalCommandResult(IDiscordInteractionCommandContext context, LocalInteractionModalResponse modal)
        : base(context)
    {
        Guard.IsNotNull(modal);

        Modal = modal;
    }

    /// <inheritdoc/>
    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return Context.Interaction.Response().SendModalAsync(Modal, cancellationToken: cancellationToken);
    }
}
