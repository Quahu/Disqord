using Disqord.Models;

namespace Disqord;

public class TransientMessageComponentInteractionMetadata(IClient client, MessageComponentInteractionMetadataJsonModel model)
    : TransientMessageInteractionMetadata(client, model), IMessageComponentInteractionMetadata
{
    public Snowflake InteractedMessageId => Model.InteractedMessageId;

    protected new MessageComponentInteractionMetadataJsonModel Model => (MessageComponentInteractionMetadataJsonModel) base.Model;
}
