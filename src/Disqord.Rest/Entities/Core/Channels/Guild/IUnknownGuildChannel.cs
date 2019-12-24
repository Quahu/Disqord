namespace Disqord
{
    public interface IUnknownGuildChannel : IGuildChannel
    {
        byte Type { get; }
    }
}
