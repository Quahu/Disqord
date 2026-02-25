using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientEntitySelectionComponentInteraction(IClient client, long receivedAt, InteractionJsonModel model)
    : TransientSelectionComponentInteraction(client, receivedAt, model), IEntitySelectionComponentInteraction
{
    /// <inheritdoc/>
    [field: MaybeNull]
    public new IReadOnlyList<Snowflake> SelectedValues
    {
        get
        {
            if (!Data.Values.HasValue)
                return ReadOnlyList<Snowflake>.Empty;

            return field ??= Data.Values.Value.ToReadOnlyList(Snowflake.Parse);
        }
    }

    /// <inheritdoc/>
    [field: MaybeNull]
    public IInteractionEntities Entities => field ??= new TransientInteractionEntities(Client, GuildId,
        Data.Resolved.GetValueOrDefault(static () => new ResolvedInteractionDataJsonModel()));
}
