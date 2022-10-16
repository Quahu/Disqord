using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientEntitySelectionComponentInteraction : TransientSelectionComponentInteraction, IEntitySelectionComponentInteraction
{
    /// <inheritdoc/>
    public new IReadOnlyList<Snowflake> SelectedValues
    {
        get
        {
            if (!Model.Data.Value.Values.HasValue)
                return ReadOnlyList<Snowflake>.Empty;

            return _selectedValues ??= Model.Data.Value.Values.Value.ToReadOnlyList(Snowflake.Parse);
        }
    }
    private IReadOnlyList<Snowflake>? _selectedValues;

    /// <inheritdoc/>
    public IInteractionEntities Entities => _entities ??= new TransientInteractionEntities(Client, GuildId,
        Model.Data.Value.Resolved.GetValueOrDefault(() => new ApplicationCommandInteractionDataResolvedJsonModel()));

    private IInteractionEntities? _entities;

    public TransientEntitySelectionComponentInteraction(IClient client, long __receivedAt, InteractionJsonModel model)
        : base(client, __receivedAt, model)
    { }
}
