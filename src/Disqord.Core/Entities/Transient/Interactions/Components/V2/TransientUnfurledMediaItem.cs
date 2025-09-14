using Disqord.Models;

namespace Disqord;

public class TransientUnfurledMediaItem(UnfurledMediaItemJsonModel model)
    : TransientEntity<UnfurledMediaItemJsonModel>(model), IUnfurledMediaItem
{
    public string Url => Model.Url;

    public static TransientUnfurledMediaItem Create(UnfurledMediaItemJsonModel model)
    {
        if (model is ResolvedUnfurledMediaItemJsonModel resolvedModel)
        {
            return new TransientResolvedUnfurledMediaItem(resolvedModel);
        }

        return new TransientUnfurledMediaItem(model);
    }
}
