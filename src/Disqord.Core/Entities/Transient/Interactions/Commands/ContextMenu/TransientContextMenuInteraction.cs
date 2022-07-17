using Disqord.Models;

namespace Disqord;

public class TransientContextMenuInteraction : TransientApplicationCommandInteraction, IContextMenuInteraction
{
    /// <inheritdoc/>
    public Snowflake TargetId => Model.Data.Value.TargetId.Value;

    public TransientContextMenuInteraction(IClient client, long __receivedAt, InteractionJsonModel model)
        : base(client, __receivedAt, model)
    { }
}