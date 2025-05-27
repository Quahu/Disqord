using Disqord.Models;

namespace Disqord;

public class TransientUnfurledMediaItem(UnfurledMediaItemJsonModel model)
    : TransientEntity<UnfurledMediaItemJsonModel>(model), IUnfurledMediaItem
{
    public string Url => Model.Url;
}
