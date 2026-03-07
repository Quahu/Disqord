using System.Collections.Generic;

namespace Disqord.Rest;

internal sealed class MessageSearchFoundMessage(
    IMessage message,
    IReadOnlyCollection<IMessage> messageWithContext)
    : IMessageSearchFoundMessage
{
    /// <inheritdoc/>
    public IMessage Message { get; } = message;

    /// <inheritdoc/>
    public IReadOnlyCollection<IMessage> MessageWithContext { get; } = messageWithContext;
}
