using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Extensions.Interactivity.Menus;

/// <inheritdoc/>
public class DefaultInteractionMenu : DefaultMenuBase
{
    /// <summary>
    ///     Gets the interaction of this menu.
    /// </summary>
    public IUserInteraction Interaction { get; }

    /// <summary>
    ///     Instantiates a new <see cref="DefaultInteractionMenu"/>.
    /// </summary>
    /// <param name="view"> The initial view. </param>
    /// <param name="interaction"> The interaction to respond with this menu for. </param>
    public DefaultInteractionMenu(ViewBase view, IUserInteraction interaction)
        : base(view)
    {
        Interaction = interaction;
    }

    /// <inheritdoc/>
    public override LocalMessageBase CreateLocalMessage()
    {
        return new LocalInteractionMessageResponse();
    }

    /// <inheritdoc/>
    protected override async Task<IUserMessage> SendLocalMessageAsync(LocalMessageBase message, CancellationToken cancellationToken)
    {
        var response = Interaction.Response();
        var interactionMessageResponse = (message as LocalInteractionMessageResponse)!;
        if (!response.HasResponded)
        {
            await response.SendMessageAsync(interactionMessageResponse, cancellationToken: cancellationToken).ConfigureAwait(false);
            return await Interaction.Followup().FetchResponseAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        return await Interaction.Followup().SendAsync(interactionMessageResponse, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}