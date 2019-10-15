namespace Disqord
{
    public interface ICustomEmoji : IEmoji, ITaggable
    {
        Snowflake Id { get; }

        bool IsAnimated { get; }

        string GetUrl(int size = 2048);
    }
}
