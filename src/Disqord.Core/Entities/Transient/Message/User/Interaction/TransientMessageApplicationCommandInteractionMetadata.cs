using Disqord.Models;
using Qommon;

namespace Disqord;

public class TransientMessageApplicationCommandInteractionMetadata(IClient client, MessageApplicationCommandInteractionMetadataJsonModel model)
    : TransientMessageInteractionMetadata(client, model), IMessageApplicationCommandInteractionMetadata
{
    /// <inheritdoc/>
    public string CommandName => Model.Name.Value;

    /// <inheritdoc/>
    public ApplicationCommandType CommandType => Model.CommandType.Value;

    /// <inheritdoc/>
    public IUser? TargetUser => field ??= Optional.ConvertOrDefault(Model.TargetUser, static (model, client) => new TransientUser(client, model), Author.Client);

    /// <inheritdoc/>
    public Snowflake? TargetMessageId => Model.TargetMessageId.GetValueOrNullable();

    protected new MessageApplicationCommandInteractionMetadataJsonModel Model => (MessageApplicationCommandInteractionMetadataJsonModel) base.Model;
}
