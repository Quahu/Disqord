using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientSelectionComponentInteraction : TransientComponentInteraction, ISelectionComponentInteraction
{
    /// <inheritdoc/>
    public new SelectionComponentType ComponentType => (SelectionComponentType) Model.Data.Value.ComponentType.Value;

    /// <inheritdoc/>
    public IReadOnlyList<string> SelectedValues
    {
        get
        {
            if (!Model.Data.Value.Values.TryGetValue(out var values))
                return ReadOnlyList<string>.Empty;

            return values;
        }
    }

    public TransientSelectionComponentInteraction(IClient client, long __receivedAt, InteractionJsonModel model)
        : base(client, __receivedAt, model)
    { }

    public new static IUserInteraction Create(IClient client, long __receivedAt, InteractionJsonModel model)
    {
        if (model.Data.Value.ComponentType.Value is >= Disqord.ComponentType.UserSelection and <= Disqord.ComponentType.ChannelSelection)
            return new TransientEntitySelectionComponentInteraction(client, __receivedAt, model);

        return new TransientSelectionComponentInteraction(client, __receivedAt, model);
    }
}
