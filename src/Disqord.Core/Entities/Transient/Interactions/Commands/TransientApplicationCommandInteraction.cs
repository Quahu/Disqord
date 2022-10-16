using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientApplicationCommandInteraction : TransientUserInteraction, IApplicationCommandInteraction
{
    /// <inheritdoc/>
    public Snowflake CommandId => Model.Data.Value.Id.Value;

    /// <inheritdoc/>
    public string CommandName => Model.Data.Value.Name.Value;

    /// <inheritdoc/>
    public ApplicationCommandType CommandType => Model.Data.Value.Type.Value;

    /// <inheritdoc/>
    public Snowflake? CommandGuildId => Model.Data.Value.GuildId.GetValueOrNullable();

    /// <inheritdoc/>
    public IInteractionEntities Entities => _entities ??= new TransientInteractionEntities(Client, GuildId,
        Model.Data.Value.Resolved.GetValueOrDefault(() => new ApplicationCommandInteractionDataResolvedJsonModel()));

    private IInteractionEntities? _entities;

    public TransientApplicationCommandInteraction(IClient client, long __receivedAt, InteractionJsonModel model)
        : base(client, __receivedAt, model)
    { }
}
