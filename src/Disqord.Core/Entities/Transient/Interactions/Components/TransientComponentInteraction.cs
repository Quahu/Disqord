using System.Diagnostics.CodeAnalysis;
using Disqord.Models;

namespace Disqord;

public class TransientComponentInteraction(IClient client, long receivedAt, InteractionJsonModel model) 
    : TransientUserInteraction(client, receivedAt, model), IComponentInteraction
{
    /// <inheritdoc/>
    public string CustomId => Data.CustomId;

    /// <inheritdoc/>
    public ComponentType ComponentType => Data.ComponentType;

    /// <inheritdoc/>
    [field: MaybeNull]
    public IUserMessage Message => field ??= new TransientUserMessage(Client, Model.Message.Value);

    private MessageComponentInteractionDataJsonModel Data => (MessageComponentInteractionDataJsonModel) Model.Data.Value;

    public new static IUserInteraction Create(IClient client, long __receivedAt, InteractionJsonModel model)
    {
        return ((MessageComponentInteractionDataJsonModel) model.Data.Value).ComponentType switch
        {
            ComponentType.StringSelection or >= ComponentType.UserSelection and <= ComponentType.ChannelSelection
                => TransientSelectionComponentInteraction.Create(client, __receivedAt, model),
            _ => new TransientComponentInteraction(client, __receivedAt, model)
        };
    }
}
