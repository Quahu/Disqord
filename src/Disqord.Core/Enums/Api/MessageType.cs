namespace Disqord.Api
{
    /// <summary>
    ///     Represents the message types.
    ///     This enumeration defines only the non-system types. The remaining types are defined in the publicly exposed <see cref="SystemMessageType"/>.
    /// </summary>
    public enum MessageType
    {
        Default = 0,

        Reply = 19,

        ApplicationCommand = 20
    }
}
