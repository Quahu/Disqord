using Disqord.Models;

namespace Disqord;

public class TransientComponentInteraction : TransientUserInteraction, IComponentInteraction
{
    /// <inheritdoc/>
    public string CustomId => Model.Data.Value.CustomId.Value;

    /// <inheritdoc/>
    public ComponentType ComponentType => Model.Data.Value.ComponentType.Value;

    /// <inheritdoc/>
    public IUserMessage Message => _message ??= new TransientUserMessage(Client, Model.Message.Value);

    private IUserMessage? _message;

    public TransientComponentInteraction(IClient client, long __receivedAt, InteractionJsonModel model)
        : base(client, __receivedAt, model)
    { }

    public new static IUserInteraction Create(IClient client, long __receivedAt, InteractionJsonModel model)
    {
        return model.Data.Value.ComponentType.Value switch
        {
            ComponentType.StringSelection or >= ComponentType.UserSelection and <= ComponentType.ChannelSelection
                => TransientSelectionComponentInteraction.Create(client, __receivedAt, model),
            _ => new TransientComponentInteraction(client, __receivedAt, model)
        };
    }
}
