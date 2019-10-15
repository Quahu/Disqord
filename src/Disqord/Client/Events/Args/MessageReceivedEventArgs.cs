namespace Disqord.Events
{
    public sealed class MessageReceivedEventArgs : DiscordEventArgs
    {
        public CachedMessage Message { get; }

        internal MessageReceivedEventArgs(CachedMessage message) : base(message.Client)
        {
            Message = message;
        }
    }
}
