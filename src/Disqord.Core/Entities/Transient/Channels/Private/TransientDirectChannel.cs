using Disqord.Models;

namespace Disqord;

/// <inheritdoc cref="IDirectChannel"/>
public class TransientDirectChannel : TransientPrivateChannel, IDirectChannel
{
    /// <inheritdoc/>
    public override string Name => Recipient.Tag;

    /// <inheritdoc/>
    public IUser Recipient => _recipient ??= new TransientUser(Client, Model.Recipients.Value[0]);

    private IUser? _recipient;

    public TransientDirectChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}
