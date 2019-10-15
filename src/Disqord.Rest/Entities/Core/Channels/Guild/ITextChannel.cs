namespace Disqord
{
    public interface ITextChannel : IGuildChannel, IMessageChannel, IMentionable, ITaggable
    {
        string Topic { get; }

        bool IsNsfw { get; }

        int Slowmode { get; }

        bool IsNews { get; }
    }
}
