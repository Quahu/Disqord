namespace Disqord.Bot.Prefixes
{
    public interface IPrefix
    {
        bool TryFind(CachedUserMessage message, out string output);
    }
}
