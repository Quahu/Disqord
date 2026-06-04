using System.Collections.Generic;

namespace Disqord.Rest;

/// <summary>
///     Represents a found message in a <see cref="IMessageSearchResponse"/>.
/// </summary>
public interface IMessageSearchFoundMessage
{
    /// <summary>
    ///     Gets the message that was found.
    /// </summary>
    IMessage Message { get; }

    /// <summary>
    ///     Gets the messages that are contextual to the found message.
    ///     This includes the found message.
    /// </summary>
    IReadOnlyCollection<IMessage> MessageWithContext { get; }
}
