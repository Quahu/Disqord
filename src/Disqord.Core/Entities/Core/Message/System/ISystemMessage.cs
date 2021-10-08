namespace Disqord
{
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
        ///     <see cref="IMessage.Content"/> is normally preferred, however if there's a new or unrecognised <see cref="SystemMessageType"/>
        ///     this can be used for manual construction of the content. See <see cref="Discord.Internal.GetSystemMessageContent(ISystemMessage, IGuild)"/>.
        /// </remarks>
        string RawContent { get; }
    }
}
