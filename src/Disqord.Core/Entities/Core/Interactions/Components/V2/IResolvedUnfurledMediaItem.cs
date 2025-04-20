namespace Disqord;

public interface IResolvedUnfurledMediaItem : IUnfurledMediaItem
{
    string? ProxyUrl { get; }

    int? Width { get; }

    int? Height { get; }

    string? ContentType { get; }

    UnfurledMediaItemLoadingState? LoadingState { get; }
}
