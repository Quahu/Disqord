namespace Disqord;

/// <summary>
///     Represents a system message sent by Discord.
/// </summary>
public interface ISystemMessage : IMessage
{
    /// <summary>
    ///     Gets the <see cref="SystemMessageType"/> of this message.
    /// </summary>
    SystemMessageType Type { get; }

    /// <summary>
    ///     Gets the raw content of this message.
    /// </summary>
    /// <remarks>
    ///     <see cref="IMessage.Content"/> is normally preferred as usually it's the content shown in the desktop Discord client,
    ///     however that's manually formatted by the library and if there's a new or unrecognized <see cref="SystemMessageType"/>
    ///     this can be used for manual construction of the content.
    ///     <br/>
    ///     See <see cref="Discord.Internal.GetSystemMessageContent(ISystemMessage, IGuild)"/>.
    /// </remarks>
    string RawContent { get; }
}