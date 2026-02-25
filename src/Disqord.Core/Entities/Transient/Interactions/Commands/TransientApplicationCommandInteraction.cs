using System.Diagnostics.CodeAnalysis;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientApplicationCommandInteraction(IClient client, long receivedAt, InteractionJsonModel model)
    : TransientUserInteraction(client, receivedAt, model), IApplicationCommandInteraction
{
    /// <inheritdoc/>
    public Snowflake CommandId => Data.Id;

    /// <inheritdoc/>
    public string CommandName => Data.Name;

    /// <inheritdoc/>
    public ApplicationCommandType CommandType => Data.Type;

    /// <inheritdoc/>
    public Snowflake? CommandGuildId => Data.GuildId.GetValueOrNullable();

    /// <inheritdoc/>
    [field: MaybeNull]
    public IInteractionEntities Entities => field ??= new TransientInteractionEntities(Client, GuildId,
        Data.Resolved.GetValueOrDefault(static () => new ResolvedInteractionDataJsonModel()));

    protected ApplicationCommandInteractionDataJsonModel Data => (ApplicationCommandInteractionDataJsonModel) Model.Data.Value;
}
