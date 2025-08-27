using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientSelectionComponentInteraction(IClient client, long receivedAt, InteractionJsonModel model) 
    : TransientComponentInteraction(client, receivedAt, model), ISelectionComponentInteraction
{
    /// <inheritdoc/>
    public new SelectionComponentType ComponentType => (SelectionComponentType) Data.ComponentType;

    /// <inheritdoc/>
    public IReadOnlyList<string> SelectedValues
    {
        get
        {
            if (!Data.Values.TryGetValue(out var values))
                return ReadOnlyList<string>.Empty;

            return values;
        }
    }

    protected MessageComponentInteractionDataJsonModel Data => (MessageComponentInteractionDataJsonModel) Model.Data.Value;

    public new static IUserInteraction Create(IClient client, long __receivedAt, InteractionJsonModel model)
    {
        if (((MessageComponentInteractionDataJsonModel) model.Data.Value).ComponentType is >= Disqord.ComponentType.UserSelection and <= Disqord.ComponentType.ChannelSelection)
        {
            return new TransientEntitySelectionComponentInteraction(client, __receivedAt, model);
        }

        return new TransientSelectionComponentInteraction(client, __receivedAt, model);
    }
}
