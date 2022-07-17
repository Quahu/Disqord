using Disqord.Models;

namespace Disqord;
// If you update any members of this class, make sure to do the same for the gateway equivalent.

public class TransientSystemMessage : TransientMessage, ISystemMessage
{
    /// <inheritdoc/>
    public SystemMessageType Type { get; }

    /// <inheritdoc/>
    public string RawContent => base.Content;

    /// <inheritdoc/>
    public override string Content => Discord.Internal.GetSystemMessageContent(this, null);

    public TransientSystemMessage(IClient client, MessageJsonModel model)
        : base(client, model)
    {
        Type = (SystemMessageType) (model.Type - 1);
    }
}