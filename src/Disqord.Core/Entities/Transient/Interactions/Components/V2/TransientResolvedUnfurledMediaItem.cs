using Disqord.Models;

namespace Disqord;

public class TransientResolvedUnfurledMediaItem(UnfurledMediaItemJsonModel model)
    : TransientUnfurledMediaItem(model), IResolvedUnfurledMediaItem
{
    public string? ProxyUrl => Model.ProxyUrl;

    public int? Width => Model.Width;

    public int? Height => Model.Height;

    public string? ContentType => Model.ContentType;

    public UnfurledMediaItemLoadingState? LoadingState => Model.LoadingState;

    public new ResolvedUnfurledMediaItemJsonModel Model => (ResolvedUnfurledMediaItemJsonModel) base.Model;
}
