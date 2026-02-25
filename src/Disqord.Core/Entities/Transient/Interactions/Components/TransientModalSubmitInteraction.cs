using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientModalSubmitInteraction(IClient client, long receivedAt, InteractionJsonModel model)
    : TransientUserInteraction(client, receivedAt, model), IModalSubmitInteraction
{
    /// <inheritdoc/>
    public string CustomId => Data.CustomId;

    /// <inheritdoc/>
    [field: MaybeNull]
    public IReadOnlyList<IModalComponent> Components => field ??= Data.Components.ToReadOnlyList(TransientModalComponent.Create);

    /// <inheritdoc/>
    [field: MaybeNull]
    public IInteractionEntities Entities => field ??= new TransientInteractionEntities(Client, GuildId,
        Data.Resolved.GetValueOrDefault(static () => new ResolvedInteractionDataJsonModel()));

    private ModalSubmitInteractionDataJsonModel Data => (ModalSubmitInteractionDataJsonModel) Model.Data.Value;
}
