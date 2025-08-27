using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientModalSubmitInteraction : TransientUserInteraction, IModalSubmitInteraction
{
    /// <inheritdoc/>
    public string CustomId => Data.CustomId;

    /// <inheritdoc/>
    [field: MaybeNull]
    public IReadOnlyList<IModalComponent> Components => field ??= Data.Components.ToReadOnlyList(Client, static (model, client) => TransientModalComponent.Create(client, model));

    private ModalSubmitInteractionDataJsonModel Data => (ModalSubmitInteractionDataJsonModel) Model.Data.Value;

    public TransientModalSubmitInteraction(IClient client, long __receivedAt, InteractionJsonModel model)
        : base(client, __receivedAt, model)
    { }
}
