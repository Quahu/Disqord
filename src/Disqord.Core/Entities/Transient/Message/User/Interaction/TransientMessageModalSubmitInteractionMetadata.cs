using Disqord.Models;

namespace Disqord;

public class TransientMessageModalSubmitInteractionMetadata(IClient client, MessageModalSubmitInteractionMetadataJsonModel model)
    : TransientMessageInteractionMetadata(client, model), IMessageModalSubmitInteractionMetadata
{
    public IMessageInteractionMetadata TriggeringInteractionMetadata => Create(Client, model.TriggeringInteractionMetadata);

    protected new MessageModalSubmitInteractionMetadataJsonModel Model => (MessageModalSubmitInteractionMetadataJsonModel) base.Model;
}
