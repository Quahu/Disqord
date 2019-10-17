namespace Disqord
{
    public interface ITextChannel : INestedChannel, IMessageChannel, IMentionable, ITaggable
    {
        string Topic { get; }

        bool IsNsfw { get; }

        int Slowmode { get; }

        bool IsNews { get; }
    }
}
