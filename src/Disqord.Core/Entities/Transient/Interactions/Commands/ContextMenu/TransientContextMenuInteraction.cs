using Disqord.Models;

namespace Disqord;

public class TransientContextMenuInteraction(IClient client, long receivedAt, InteractionJsonModel model)
    : TransientApplicationCommandInteraction(client, receivedAt, model), IContextMenuInteraction
{
    /// <inheritdoc/>
    public Snowflake TargetId => Data.TargetId.Value;
}
