namespace Disqord
{
    public interface IGuildSticker : ISticker, IGuildEntity
    {
        bool IsAvailable { get; }

        IUser Creator { get; }
    }
}