namespace Disqord
{
    public interface ISystemMessage : IMessage
    {
        SystemMessageType Type { get; }

        string RawContent { get; }
    }
}
